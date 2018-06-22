using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ConsoleBox.Test
{
    using Terminal;

    //https://stackoverflow.com/questions/42235401/unit-testing-internal-methods-in-vs2017-net-standard-library
    [TestClass]
    public class VTInputTests
    {
        const char ESC = '\x1b';

        public class TestEmitter
        {
            public readonly InputEventEmitter Emitter;

            public readonly List<MouseEvent> MouseMoves = new List<MouseEvent>();
            public readonly List<MouseButtonEvent> MouseDowns = new List<MouseButtonEvent>();
            public readonly List<MouseButtonEvent> MouseUps = new List<MouseButtonEvent>();
            public readonly List<MouseButtonEvent> MouseClicks = new List<MouseButtonEvent>();
            public readonly List<MouseWheelEvent> MouseScrolls = new List<MouseWheelEvent>();
            public readonly List<ConsoleKeyInfo> Keys = new List<ConsoleKeyInfo>();

            public TestEmitter()
            {
                Emitter = new InputEventEmitter(this);
                Emitter.KeyEvent += (s, e) => Keys.Add(e);
                Emitter.MouseButtonDown += (s, e) => MouseDowns.Add(e);
                Emitter.MouseButtonUp += (s, e) => MouseUps.Add(e);
                Emitter.MouseClick += (s, e) => MouseClicks.Add(e);
                Emitter.MouseMove += (s, e) => MouseMoves.Add(e);
                Emitter.MouseWheel += (s, e) => MouseScrolls.Add(e);
            }
        }

        public static ArraySegment<char> CreateBufferFromString(string data, int bufferSize)
        {
            var buffer = new char[bufferSize];
            for (var i = 0; i < data.Length; i++)
            {
                buffer[i] = data[i];
            }
            return new ArraySegment<char>(buffer, 0, data.Length);
        }

        //[TestMethod]
        //public void ShouldOutputOnUnknownEscape()
        //{
        //    var test = new TestEmitter();

        //    var data = $"{ESC}abcd";
        //    var buffer = CreateBufferFromString(data, 16);

        //    var parser = new TermControlParser(test.Emitter);
        //    parser.Process(buffer);

        //    Assert.AreEqual(data.Length, test.Keys.Count);
        //    Assert.IsTrue(test.Keys[0].Key == ConsoleKey.Escape);
        //}

        [TestMethod]
        public void ShouldOutputSingleEscape()
        {
            var test = new TestEmitter();

            var data = $"{ESC}";
            var buffer = CreateBufferFromString(data, 16);
            var map = new InputMapper();
            var keys = new KeysMap(test.Emitter);
            keys.Register(map);

            map.Process(buffer);

            Assert.AreEqual(data.Length, test.Keys.Count);
            Assert.IsTrue(test.Keys[0].Key == ConsoleKey.Escape);
        }

        [TestMethod]
        public void ShouldParseExtendedMouseMove()
        {
            var test = new TestEmitter();

            var data = ESC + "[<35;52;1m";
            var buffer = CreateBufferFromString(data, 16);

            var parser = new ExtendedMouseModeEncoding(test.Emitter);
            var r = parser.Process(buffer);

            Assert.AreEqual(data.Length, r);
            Assert.AreEqual(1, test.MouseMoves.Count);
        }

        [TestMethod]
        public void ShouldParseExtendedMouseClick()
        {
            var test = new TestEmitter();

            var md = ESC + "[<0;52;1M";
            var mu = ESC + "[<3;52;1m";

            var parser = new ExtendedMouseModeEncoding(test.Emitter);
            var rd = parser.Process(CreateBufferFromString(md, 16));
            var ru = parser.Process(CreateBufferFromString(mu, 16));

            Assert.AreEqual(md.Length, rd);
            Assert.AreEqual(mu.Length, ru);
            Assert.AreEqual(1, test.MouseMoves.Count);  // same coords, should not move
            Assert.AreEqual(1, test.MouseDowns.Count);
            Assert.AreEqual(1, test.MouseUps.Count);
            Assert.AreEqual(1, test.MouseClicks.Count);
        }

        [TestMethod]
        public void ShouldParseClumpedClicks()
        {
            var test = new TestEmitter();

            var md = ESC + "[<0;52;1M";
            var mu = ESC + "[<3;52;1m";

            var map = new InputMapper();
            var c = new ExtendedMouseModeEncoding(test.Emitter);
            c.Register(map);

            // as long as the parser gets the entire sequence, additional chars shouldn't break anything
            var clump = "123" + md + "456" + mu + "789";
            map.Process(CreateBufferFromString(clump, 32));

            Assert.AreEqual(1, test.MouseMoves.Count);  // same coords, should not move
            Assert.AreEqual(1, test.MouseDowns.Count);
            Assert.AreEqual(1, test.MouseUps.Count);
            Assert.AreEqual(1, test.MouseClicks.Count);
            Assert.AreEqual(9, test.Keys.Count);
        }
    }
}
