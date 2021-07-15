#pragma once

using namespace System;

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
