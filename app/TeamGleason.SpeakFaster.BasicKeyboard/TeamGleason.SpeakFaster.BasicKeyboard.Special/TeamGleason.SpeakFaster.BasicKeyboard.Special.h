#pragma once

using namespace System;

namespace TeamGleason { namespace SpeakFaster { namespace BasicKeyboard { namespace Special {
	public ref class InteropHelper
	{
	public:
		static void SendCharacter(wchar_t ch)
		{
			INPUT inputs[2];
			ZeroMemory(inputs, sizeof(inputs));

			inputs[0].type = INPUT_KEYBOARD;
			inputs[0].ki.wVk = ch;

			inputs[1].type = INPUT_KEYBOARD;
			inputs[1].ki.wVk = ch;
			inputs[1].ki.dwFlags = KEYEVENTF_KEYUP;

			UINT uSent = SendInput(ARRAYSIZE(inputs), inputs, sizeof(INPUT));
		}

		static void SetMainWindowStyle(IntPtr handle)
		{
			auto hWnd = (HWND)(void*)handle;
			SetWindowLong(hWnd, GWL_EXSTYLE,
				GetWindowLong(hWnd, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
		}
	};
} } } }
