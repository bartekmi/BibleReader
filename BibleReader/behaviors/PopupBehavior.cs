using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace BibleReader.behaviors {
    public class PopupBehavior {

        private enum State {
            Default,
            TimingToOpen,
            PopupOpen,
            TimingToClose,
        }

        private const int TIME_TO_OPEN = 250;       // milliseconds
        private const int TIME_TO_CLOSE = 150;      // milliseconds

        private State _state = State.Default;
        private FrameworkElement _target;
        private Func<FrameworkElement> _popupContentCreator;
        private FrameworkElement _popupContent;
        private Popup _popup;
        private DispatcherTimer _timer;

        private PopupBehavior(FrameworkElement target, Func<FrameworkElement> popupContentCreator) {
            _target = target;
            _popupContentCreator = popupContentCreator;
            _timer = new DispatcherTimer();
            _timer.Tick += (s, e) => TimerFired();

            target.MouseEnter += MouseEnterTarget;
            target.MouseLeave += MouseLeaveTarget;
        }

        private void MouseEnterTarget(object s, MouseEventArgs e) {
            _timer.Interval = TimeSpan.FromMilliseconds(TIME_TO_OPEN);
            _timer.Start();
            _state = State.TimingToOpen;
        }

        private void MouseLeaveTarget(object s, MouseEventArgs e) {
            if (_state == State.TimingToOpen) {         // User moved mouse away before we ever showed the Popup => Go back to default state
                _timer.Stop();
                _state = State.Default;
            } else if (_state == State.PopupOpen) {     // User moved mouse off target (and may be moving towards Popup) => Start timer; if user enters popup before it expires, popup will remain open
                _timer.Interval = TimeSpan.FromMilliseconds(TIME_TO_CLOSE);
                _timer.Start();
                _state = State.TimingToClose;
            }
        }

        private void TimerFired() {
            _timer.Stop();

            if (_state == State.TimingToOpen) {             // Timer fired while hovering target => Open Popup
                if (_popup == null)
                    CreatePopup();
                _popup.IsOpen = true;
                _state = State.PopupOpen;
            } else if (_state == State.TimingToClose) {     // Timer fired after user left target but before he entered popup => Close popup.
                _popup.IsOpen = false;
                _state = State.Default;
            }
        }

        private void MouseEnterPopup() {        // User entered popup before timer expired... Keep popup open
            _timer.Stop();
            _state = State.PopupOpen;
        }

        private void MouseLeavePopup(MouseEventArgs e) {        // User left popup => close it
            if (IsInside((FrameworkElement)_popup.Child, e)) {
                e.Handled = true;
                return;
            }

            _popup.IsOpen = false;
            _state = State.Default;
        }

        // Reason for not using <=, >= is it fails to close pop-up
        private static bool IsInside(FrameworkElement element, MouseEventArgs e) {
            Point point = e.GetPosition(element);
            return point.X > 0.0 && point.X < element.ActualWidth &&
                point.Y > 0.0 && point.Y < element.ActualHeight;
        }

        private void CreatePopup() {
            Grid grid = new Grid() {
                Background = Brushes.White,
            };

            if (_popupContent == null) {
                _popupContent = _popupContentCreator();
            }

            grid.Children.Add(_popupContent);

            _popup = new Popup() {
                Child = grid,
                Placement = PlacementMode.MousePoint,
                HorizontalOffset = 3.0,     // Without the offsets, the pop-up appears directly under the mouse, making it impossible to click and select whatever was under it
                VerticalOffset = 3.0,
                AllowsTransparency = true,
            };
            _popup.MouseEnter += (s, e) => MouseEnterPopup();
            _popup.MouseLeave += (s, e) => MouseLeavePopup(e);
        }


        public static void Attach(FrameworkElement target, Func<FrameworkElement> popupContentCreator) {
            new PopupBehavior(target, popupContentCreator);
        }
    }
}
