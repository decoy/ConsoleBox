namespace ConsoleBox.Windows
{
    internal static class Constants
    {
        internal const int foreground_blue = 0x1;
        internal const int foreground_green = 0x2;
        internal const int foreground_red = 0x4;
        internal const int foreground_intensity = 0x8;
        internal const int background_blue = 0x10;
        internal const int background_green = 0x20;
        internal const int background_red = 0x40;
        internal const int background_intensity = 0x80;

        internal const int std_input_handle = -0xa;
        internal const int std_output_handle = -0xb;

        internal const int key_event = 0x1;
        internal const int mouse_event = 0x2;
        internal const int window_buffer_size_event = 0x4;

        internal const int enable_window_input = 0x8;
        internal const int enable_mouse_input = 0x10;
        internal const int enable_extended_flags = 0x80;

        internal const int vk_f1 = 0x70;
        internal const int vk_f2 = 0x71;
        internal const int vk_f3 = 0x72;
        internal const int vk_f4 = 0x73;
        internal const int vk_f5 = 0x74;
        internal const int vk_f6 = 0x75;
        internal const int vk_f7 = 0x76;
        internal const int vk_f8 = 0x77;
        internal const int vk_f9 = 0x78;
        internal const int vk_f10 = 0x79;
        internal const int vk_f11 = 0x7a;
        internal const int vk_f12 = 0x7b;
        internal const int vk_insert = 0x2d;
        internal const int vk_delete = 0x2e;
        internal const int vk_home = 0x24;
        internal const int vk_end = 0x23;
        internal const int vk_pgup = 0x21;
        internal const int vk_pgdn = 0x22;
        internal const int vk_arrow_up = 0x26;
        internal const int vk_arrow_down = 0x28;
        internal const int vk_arrow_left = 0x25;
        internal const int vk_arrow_right = 0x27;
        internal const int vk_backspace = 0x8;
        internal const int vk_tab = 0x9;
        internal const int vk_enter = 0xd;
        internal const int vk_esc = 0x1b;
        internal const int vk_space = 0x20;

        internal const short vk_alt = 0x12;

        internal const int left_alt_pressed = 0x2;
        internal const int left_ctrl_pressed = 0x8;
        internal const int right_alt_pressed = 0x1;
        internal const int right_ctrl_pressed = 0x4;
        internal const int shift_pressed = 0x10;

        internal const uint generic_read = 0x80000000;
        internal const int generic_write = 0x40000000;
        internal const int console_textmode_buffer = 0x1;

        internal const int STD_OUTPUT_HANDLE = -11;
        internal const int STD_INPUT_HANDLE = -10;
        internal const int STD_ERROR_HANDLE = -12;

        internal const int mouse_lmb = 0x1;
        internal const int mouse_rmb = 0x2;
        internal const int mouse_mmb = 0x4 | 0x8 | 0x10;

        internal const ushort FOREGROUND_BLUE = 0x0001;
        internal const ushort FOREGROUND_GREEN = 0x0002;
        internal const ushort FOREGROUND_RED = 0x0004;
        internal const ushort FOREGROUND_INTENSITY = 0x0008;
        internal const ushort BACKGROUND_BLUE = 0x0010;
        internal const ushort BACKGROUND_GREEN = 0x0020;
        internal const ushort BACKGROUND_RED = 0x0040;
        internal const ushort BACKGROUND_INTENSITY = 0x0080;

        internal const ushort COMMON_LVB_LEADING_BYTE = 0x0100;
        internal const ushort COMMON_LVB_TRAILING_BYTE = 0x0200;
        internal const ushort COMMON_LVB_GRID_HORIZONTAL = 0x0400;
        internal const ushort COMMON_LVB_GRID_LVERTICAL = 0x0800;
        internal const ushort COMMON_LVB_GRID_RVERTICAL = 0x1000;
        internal const ushort COMMON_LVB_REVERSE_VIDEO = 0x4000;
        internal const ushort COMMON_LVB_UNDERSCORE = 0x8000;

        internal const uint ENABLE_ECHO_INPUT = 0x0004;
        internal const uint ENABLE_EXTENDED_FLAGS = 0x0080;
        internal const uint ENABLE_INSERT_MODE = 0x0020;
        internal const uint ENABLE_LINE_INPUT = 0x0002;
        internal const uint ENABLE_MOUSE_INPUT = 0x0010;
        internal const uint ENABLE_PROCESSED_INPUT = 0x0001;
        internal const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        internal const uint ENABLE_WINDOW_INPUT = 0x0008;
        internal const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

        internal const uint ENABLE_PROCESSED_OUTPUT = 0x0001;
        internal const uint ENABLE_WRAP_AT_EOL_OUTPUT = 0x0002;
        internal const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        internal const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
        internal const uint ENABLE_LVB_GRID_WORLDWIDE = 0x0010;
    }
}
