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
        private List<TimelineFrame> frames;
        bool isReverse;
        bool isSnap;

        public int CurrentIndex { get; set; }
        public readonly int MaxIndex = 1000;


        public Timeline()
        {
            isReverse = false;
            frames = new List<TimelineFrame>();
            this.Advance();
            CurrentIndex = 0;
        }

        public bool IsReverse { get => isReverse; set => isReverse = value; }
        public bool IsSnap { get => isSnap; set => isSnap = value; }

        public TimelineFrame currentFrame { get => frames[CurrentIndex]; }

        public void Advance()
        {
            CurrentIndex += 1;
            if (CurrentIndex >= frames.Count)
            {
                frames.Add(new TimelineFrame());
            }
        }
        public void FallBack()
        {
            CurrentIndex -= 1;
            CurrentIndex = (CurrentIndex < 0) ? 0 : CurrentIndex;
        }

        public void ForgetTheFuture(EntityIdentifier id)
        {
            foreach(var f in this.frames.Skip(CurrentIndex + 1))
            {
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
    }
}
