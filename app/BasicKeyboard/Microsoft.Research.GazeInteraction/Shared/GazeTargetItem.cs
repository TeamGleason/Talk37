// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Input.GazeInteraction
{
    public abstract class GazeTargetItem
    {
        public  TimeSpan DetailedTime { get; set; }

        public TimeSpan OverflowTime { get; set; }

        public TimeSpan ElapsedTime
        {
            get { return DetailedTime + OverflowTime; }
        }

        public TimeSpan NextStateTime { get; set; }

        public TimeSpan LastTimestamp { get; set; }

        public PointerState ElementState { get; set; }

        public int RepeatCount { get; set; }

        public int MaxDwellRepeatCount { get; set; }

        protected GazeTargetItem()
        {
        }

        public void RaiseGazePointerEvent(PointerState state, TimeSpan elapsedTime)
        {
            if (state == PointerState.Dwell)
            {
                Invoke();
            }
        }

        public abstract TimeSpan GetElementStateDelay(PointerState pointerState, TimeSpan defaultValue);

        public abstract TimeSpan GetElementRepeatDelay(TimeSpan defaultValue);

        public abstract void Invoke();

        public virtual bool IsInvokable
        {
            get { return true; }
        }

        public void Reset(TimeSpan nextStateTime)
        {
            ElementState = PointerState.PreEnter;
            DetailedTime = TimeSpan.Zero;
            OverflowTime = TimeSpan.Zero;
            NextStateTime = nextStateTime;
            RepeatCount = 0;
            MaxDwellRepeatCount = GetMaxDwellRepeatCount();
        }

        public abstract int GetMaxDwellRepeatCount();

        public void GiveFeedback()
        {
            if (_nextStateTime != NextStateTime)
            {
                _prevStateTime = _nextStateTime;
                _nextStateTime = NextStateTime;
            }

            if (ElementState != _notifiedPointerState)
            {
                switch (ElementState)
                {
                    case PointerState.Enter:
                        RaiseProgressEvent(DwellProgressState.Fixating);
                        break;

                    case PointerState.Dwell:
                    case PointerState.Fixation:
                        RaiseProgressEvent(DwellProgressState.Progressing);
                        break;

                    case PointerState.Exit:
                    case PointerState.PreEnter:
                        RaiseProgressEvent(DwellProgressState.Idle);
                        break;
                }

                _notifiedPointerState = ElementState;
            }
            else if (ElementState == PointerState.Dwell || ElementState == PointerState.Fixation)
            {
                if (RepeatCount <= MaxDwellRepeatCount)
                {
                    RaiseProgressEvent(DwellProgressState.Progressing);
                }
                else
                {
                    RaiseProgressEvent(DwellProgressState.Complete);
                }
            }
        }

        internal void RaiseProgressEvent(DwellProgressState state)
        {
            if (_notifiedProgressState != state || state == DwellProgressState.Progressing)
            {
                var feedbackProgress = state == DwellProgressState.Progressing ?
                    ((double)(ElapsedTime - _prevStateTime).Ticks) / (_nextStateTime - _prevStateTime).Ticks :
                    0.0;
                ShowFeedback(state, feedbackProgress);
            }

            _notifiedProgressState = state;
        }

        protected abstract void ShowFeedback(DwellProgressState state, double progress);

        private PointerState _notifiedPointerState = PointerState.Exit;
        private TimeSpan _prevStateTime;
        private TimeSpan _nextStateTime;
        private DwellProgressState _notifiedProgressState = DwellProgressState.Idle;
    }
}