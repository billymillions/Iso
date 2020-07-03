using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TimelineIso
{
    public class TimelineEvent
    {
        public readonly EntityIdentifier id;
        public readonly ValueType item;

        public TimelineEvent(EntityIdentifier id, ValueType item)
        {
            this.id = id;
            this.item = item;
        }
    }

    public class TimelineFrame : List<TimelineEvent>
    {

    }

    [System.Serializable]
    public class Timeline
    {
        //private List<TimelineFrame> frames;
        private TimelineFrame[] frames;
        bool isReverse;
        bool isSnap;
        bool isPrepare;

        public readonly int TimelineSize = 2000;
        public int CurrentIndex = 0;
        public int StartIndex = 0;

        public Timeline()
        {
            isReverse = false;
            frames = new TimelineFrame[TimelineSize];
            for (int i = 0; i<TimelineSize; i++)
            {
                frames[i] = new TimelineFrame();
            }
            CurrentIndex = 0;
        }

        public bool IsReverse { get => isReverse; set => isReverse = value; }
        public bool IsSnap { get => isSnap; set => isSnap = value; }
        public bool IsPrepare { get => isPrepare; set => isPrepare = value; }

        public TimelineFrame currentFrame { get => frames[CurrentIndex]; }

        public void Advance()
        {
            CurrentIndex += 1;
            CurrentIndex %= TimelineSize;
            if (CurrentIndex == StartIndex)
            {
                StartIndex = (CurrentIndex + 1) % TimelineSize;
                frames[StartIndex] = new TimelineFrame();
            }
        }

        public void FallBack()
        {
            if (CurrentIndex == StartIndex)
            {
                CurrentIndex = StartIndex;
                return;
            }
            CurrentIndex = (CurrentIndex - 1) % TimelineSize;
        }

        public void ForgetTheFuture(EntityIdentifier id)
        {
            currentFrame.RemoveAll((x) => x.id.Equals(id));
            for (int i = (CurrentIndex + 1) % TimelineSize; i != StartIndex; i = (i + 1) % TimelineSize)
            {
                var f = frames[i];
                f.RemoveAll((x) => x.id.Equals(id));
            }
        }

        public void Forget<T>(EntityIdentifier id)
        {
            currentFrame.RemoveAll(x => x.id.Equals(id) && x is T);
        }

        public void Replace<T>(EntityIdentifier id, T item) where T : struct
        {
            this.Forget<T>(id);
            this.Remember(id, item);
        }

        public void Remember<T>(EntityIdentifier id, T item) where T : struct
        {
            frames[this.CurrentIndex].Add(new TimelineEvent(id, item));
        }

        public T? Restore<T>(EntityIdentifier id) where T : struct
        {
            foreach (var f in frames[CurrentIndex])
            {
                if (f.id.Equals(id) && f.item is T)
                {
                    return new Nullable<T>((T)f.item);
                }

            }
            return null;
        }
        public void RestoreIfValue<T>(EntityIdentifier id, ref T result) where T : struct
        {
            var maybeValue = Restore<T>(id);
            if (maybeValue.HasValue)
            {
                result = maybeValue.Value;
            }
        }

        internal void SnapBack()
        {
            this.CurrentIndex = StartIndex;
            this.isSnap = true;
        }

        internal void PrepareSnap()
        {
            this.isPrepare = true;
        }
    }
}
