#pragma once

using namespace System;
using namespace System::Windows::Input;
using namespace TeamGleason::SpeakFaster::BasicKeyboard::Layout::Standard;

namespace TeamGleason {
	namespace SpeakFaster {
		namespace BasicKeyboard {
			namespace Special {
				HHOOK s_oldHook;
				static LRESULT CALLBACK LowLevelKeyboardProc(
					_In_ int    nCode,
					_In_ WPARAM wParam,
					_In_ LPARAM lParam);

				public ref class InteropHelper
				{
				public:
					static property bool IsShift
					{
						bool get()
						{
							auto value = GetKeyState(VK_SHIFT);
							return value < 0;
						}
					}

					static property bool IsControl
					{
						bool get()
						{
							auto value = GetKeyState(VK_CONTROL);
							return value < 0;
						}
					}

					static property bool IsCapsLock
					{
						bool get()
						{
							auto value = GetKeyState(VK_CAPITAL);
							return (value & 1) != 0;
						}
					}

					static event EventHandler^ StateChange
					{
						void add(EventHandler^ p)
						{
							if (_stateChangeListenerCount == 0)
							{
								StartListening();
							}
							_stateChangeListenerCount++;

							_stateChange = static_cast<EventHandler^> (Delegate::Combine(_stateChange, p));
						}

						void remove(EventHandler^ p)
						{
							// TODO: Something's wrong here, it doesn't appear to remove the event handler.
							_stateChange = static_cast<EventHandler^> (Delegate::Remove(_stateChange, p));

							_stateChangeListenerCount--;
							if (_stateChangeListenerCount == 0)
							{
								StopListening();
							}
						}

						void raise(Object^ sender, EventArgs^ args)
						{
							if (_stateChange != nullptr)
							{
								_stateChange->Invoke(sender, args);
							}
						}
					}

					static void SendKey(bool sendDown, bool sendUp, KeyName key)
					{
						WORD code = 0;

						switch (key)
						{
						case KeyName::Backspace: code = VK_BACK; break;
						case KeyName::Tab: code = VK_TAB; break;
						case KeyName::Enter: code = VK_RETURN; break;
						case KeyName::Space: code = VK_SPACE; break;
						case KeyName::Home: code = VK_HOME; break;
						case KeyName::End: code = VK_END; break;
						case KeyName::Delete: code = VK_DELETE; break;
						case KeyName::PageUp: code = VK_PRIOR; break;
						case KeyName::PageDown: code = VK_NEXT; break;
						case KeyName::ArrowUp: code = VK_UP; break;
						case KeyName::ArrowDown: code = VK_DOWN; break;
						case KeyName::ArrowLeft: code = VK_LEFT; break;
						case KeyName::ArrowRight: code = VK_RIGHT; break;
						case KeyName::F1: code = VK_F1; break;
						case KeyName::F2: code = VK_F2; break;
						case KeyName::F3: code = VK_F3; break;
						case KeyName::F4: code = VK_F4; break;
						case KeyName::F5: code = VK_F5; break;
						case KeyName::F6: code = VK_F6; break;
						case KeyName::F7: code = VK_F7; break;
						case KeyName::F8: code = VK_F8; break;
						case KeyName::F9: code = VK_F9; break;
						case KeyName::F10: code = VK_F10; break;
						case KeyName::F11: code = VK_F11; break;
						case KeyName::F12: code = VK_F12; break;
						case KeyName::Escape: code = VK_ESCAPE; break;
						}

						if (code != 0 && (sendDown || sendUp))
						{
							INPUT inputs[2];
							ZeroMemory(inputs, sizeof(inputs));

							inputs[0].type = INPUT_KEYBOARD;
							inputs[0].ki.wVk = code;
							inputs[0].ki.dwFlags = sendDown ? 0 : KEYEVENTF_KEYUP;

							inputs[1].type = INPUT_KEYBOARD;
							inputs[1].ki.wVk = code;
							inputs[1].ki.dwFlags = KEYEVENTF_KEYUP;

							UINT uSent = SendInput((sendUp && sendDown) ? 2 : 1, inputs, sizeof(INPUT));
						}
					}

					static void SendText(bool isShift, bool isCtrl, bool isAlt, bool isWindows, String^ text)
					{
						auto length = text->Length;
						for (auto i = 0; i < length; i++)
						{
							auto ch = text[i];

							auto code = VkKeyScan(ch);

							if ((code & 0xFFFF) != 0xFFFF)
							{
								auto isShiftNeeded = isShift || !!(code & 0x0100);
								auto isCtrlNeeded = isCtrl || !!(code & 0x0200);
								auto isAltNeeded = isAlt || !!(code & 0x0400);
								//auto isHankakuNeeded = !!(code & 0x0800);

								INPUT inputs[8];
								ZeroMemory(inputs, sizeof(inputs));

								auto count = 0;

								if (isShiftNeeded)
								{
									inputs[count].type = INPUT_KEYBOARD;
									inputs[count].ki.wVk = VK_SHIFT;
									count++;
								}

								if (isCtrlNeeded)
								{
									inputs[count].type = INPUT_KEYBOARD;
									inputs[count].ki.wVk = VK_CONTROL;
									count++;
								}

								if (isAltNeeded)
								{
									inputs[count].type = INPUT_KEYBOARD;
									inputs[count].ki.wVk = VK_MENU;
									count++;
								}

								inputs[count].type = INPUT_KEYBOARD;
								inputs[count].ki.wVk = code & 0xFF;
								count++;

								auto countdown = count;
								while (countdown != 0)
								{
									countdown--;

									inputs[count].type = inputs[countdown].type;
									inputs[count].ki.wVk = inputs[countdown].ki.wVk;
									inputs[count].ki.dwFlags = inputs[countdown].ki.dwFlags ^ KEYEVENTF_KEYUP;
									count++;
								}

								UINT uSent = SendInput(count, inputs, sizeof(INPUT));
							}
							else
							{
								System::Diagnostics::Debug::WriteLine(String::Format("Cannot send {0}", ch));
							}
						}
					}

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

					static void RaiseStateChange()
					{
						if (_stateChange != nullptr)
						{
							_stateChange->Invoke(nullptr, EventArgs::Empty);
						}
					}

					static void LookForStateChange()
					{
						auto changed = IsChanged(&_isShift, IsShift) ||
							IsChanged(&_isControl, IsControl) ||
							IsChanged(&_isCapsLock, IsCapsLock);
						if (changed)
						{
							RaiseStateChange();
						}
					}

				private:
					static EventHandler^ _stateChange = nullptr;
					static int _stateChangeListenerCount = 0;
					static bool _isShift = false;
					static bool _isControl = false;
					static bool _isCapsLock = false;

					static bool IsChanged(interior_ptr<bool> previous, bool now)
					{
						auto changed = *previous != now;
						if (changed)
						{
							*previous = now;
						}
						return changed;
					}

					static void StartListening()
					{
						s_oldHook = SetWindowsHookEx(WH_KEYBOARD_LL, LowLevelKeyboardProc, 0, 0);
					}

					static void StopListening()
					{
						UnhookWindowsHookEx(s_oldHook);
					}
				};

				static LRESULT CALLBACK LowLevelKeyboardProc(
					_In_ int    nCode,
					_In_ WPARAM wParam,
					_In_ LPARAM lParam)
				{
					InteropHelper::LookForStateChange();

					auto value = CallNextHookEx(s_oldHook, nCode, wParam, lParam);

					return value;
				}
			}
		}
	}
}
