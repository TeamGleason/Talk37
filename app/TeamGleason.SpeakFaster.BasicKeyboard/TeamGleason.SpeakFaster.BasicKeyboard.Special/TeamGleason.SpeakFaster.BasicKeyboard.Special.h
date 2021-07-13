#pragma once

using namespace System;

namespace TeamGleason { namespace SpeakFaster { namespace BasicKeyboard { namespace Special {
	public ref class InteropHelper
	{
	public:
		static void SendKeystroke()
		{
			MessageBeep(0xFFFFFFFF);
		}

		static void SetMainWindowStyle(IntPtr handle)
		{
			auto hWnd = (HWND)(void*)handle;
			SetWindowLong(hWnd, GWL_EXSTYLE,
				GetWindowLong(hWnd, GWL_EXSTYLE) | WS_EX_NOACTIVATE);
		}
	};
} } } }
