using System;

namespace ConsoleBox.Terminal
{
    public interface ITermCommand
    {
        int Process(InputEventEmitter emit, ArraySegment<ConsoleKeyInfo> buffer);
    }
}
