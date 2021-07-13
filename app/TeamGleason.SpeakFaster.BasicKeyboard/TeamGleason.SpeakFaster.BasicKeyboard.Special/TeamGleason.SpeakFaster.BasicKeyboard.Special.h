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
	};
} } } }
