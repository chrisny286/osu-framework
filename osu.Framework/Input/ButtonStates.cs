// Copyright (c) 2007-2018 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu-framework/master/LICENCE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Extensions.TypeExtensions;

namespace osu.Framework.Input
{
    public class ButtonStates<TButton> : IEnumerable<TButton>
    where TButton : struct
    {
        private List<TButton> pressedButtons { get; set; } = new List<TButton>();

        public ButtonStates<TButton> Clone()
        {
            var clone = (ButtonStates<TButton>)MemberwiseClone();
            clone.pressedButtons = new List<TButton>(pressedButtons);
            return clone;
        }

        public bool IsPressed(TButton button) => pressedButtons.Contains(button);

        /// <summary>
        /// Set the state of <param name="button"></param>.
        /// </summary>
        /// <returns>Wether the button state actually changed.</returns>
        public bool SetPressed(TButton button, bool pressed)
        {
            if (pressedButtons.Contains(button) == pressed)
                return false;

            if (pressed)
                pressedButtons.Add(button);
            else
                pressedButtons.Remove(button);
            return true;
        }

        public bool HasAnyButtonPressed => pressedButtons.Any();

        public (IEnumerable<TButton> Released, IEnumerable<TButton> Pressed) EnumerateDifference(ButtonStates<TButton> lastButtons)
        {
            return (lastButtons.pressedButtons.Except(pressedButtons), pressedButtons.Except(lastButtons.pressedButtons));
        }

        public void Set(ButtonStates<TButton> other)
        {
            pressedButtons.Clear();
            pressedButtons.AddRange(other.pressedButtons);
        }

        public override string ToString()
        {
            return $@"{GetType().ReadableName()}({String.Join(" ", pressedButtons)})";
        }

        public IEnumerator<TButton> GetEnumerator()
        {
            return ((IEnumerable<TButton>)pressedButtons).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TButton>)pressedButtons).GetEnumerator();
        }

        // for collection initializer
        public void Add(TButton button) => SetPressed(button, true);
    }
}
