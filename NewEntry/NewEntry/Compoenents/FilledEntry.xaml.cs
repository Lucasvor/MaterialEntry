using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace NewEntry.Components
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilledEntry : Grid
    {
        private string tempIcon;

        // BindableProperties
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(FilledEntry),
            default(string),
            BindingMode.TwoWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.customEntry.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty PlaceholderTextProperty = BindableProperty.Create(
            nameof(PlaceholderText),
            typeof(string),
            typeof(FilledEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.placeholderText.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty HelperTextProperty = BindableProperty.Create(
            nameof(HelperText),
            typeof(string),
            typeof(FilledEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.helperText.Text = (string)newValue;

                if (view.errorText.IsVisible)
                    view.helperText.IsVisible = false;
                else
                    view.helperText.IsVisible = !string.IsNullOrEmpty(view.helperText.Text);
            }
        );

        public static readonly BindableProperty ErrorTextProperty = BindableProperty.Create(
            nameof(ErrorText),
            typeof(string),
            typeof(FilledEntry),
            default(string),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.errorText.Text = (string)newValue;
            }
        );

        public static readonly BindableProperty LeadingIconProperty = BindableProperty.Create(
            nameof(LeadingIcon),
            typeof(ImageSource),
            typeof(FilledEntry),
            default(ImageSource),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.leadingIcon.Source = (ImageSource)newValue;

                view.leadingIcon.IsVisible = !view.leadingIcon.Source.IsEmpty;
            }
        );

        public static readonly BindableProperty TrailingIconProperty = BindableProperty.Create(
            nameof(TrailingIcon),
            typeof(string),
            typeof(FilledEntry),
            default(string),
            BindingMode.TwoWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;
                if (!string.IsNullOrWhiteSpace(newValue.ToString()))
                    view.trailingIcon.Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(newValue.ToString());

                view.trailingIcon.IsVisible = view.trailingIcon.Data != null;
            }
        );

        public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(
            nameof(HasError),
            typeof(bool),
            typeof(FilledEntry),
            default(bool),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.errorText.IsVisible = (bool)newValue;

                //view.containerFrame.BorderColor = view.errorText.IsVisible ? Color.Red : Color.Black;

                view.helperText.IsVisible = !view.errorText.IsVisible;
                view.BorderColor = view.errorText.IsVisible ? Color.Red : view.BorderColor;
                view.placeholderText.TextColor = view.errorText.IsVisible ? Color.Red : Color.Gray;

                view.PlaceholderText = view.errorText.IsVisible ? $"{view.PlaceholderText}*" : view.PlaceholderText;

                if (view.TrailingIcon != null && !string.IsNullOrEmpty(view.TrailingIcon))
                    view.tempIcon = view.TrailingIcon;

                view.TrailingIcon = view.errorText.IsVisible
                    ? "m12.002 21.534c5.518 0 9.998-4.48 9.998-9.998s-4.48-9.997-9.998-9.997c-5.517 0-9.997 4.479-9.997 9.997s4.48 9.998 9.997 9.998zm0-8c-.414 0-.75-.336-.75-.75v-5.5c0-.414.336-.75.75-.75s.75.336.75.75v5.5c0 .414-.336.75-.75.75zm-.002 3c-.552 0-1-.448-1-1s.448-1 1-1 1 .448 1 1-.448 1-1 1z"
                    : view.tempIcon;
                view.trailingIcon.Stroke = Brush.Red;

                if (view.ClearText)
                    view.trailingIcon.IsVisible = false;

                if(!view.ClearText)
                    view.trailingIcon.IsVisible = view.errorText.IsVisible;
            }
        );
        public static readonly BindableProperty ClearTextProperty = BindableProperty.Create(
            nameof(ClearText),
            typeof(bool),
            typeof(FilledEntry),
            default(bool),
            BindingMode.TwoWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                //view.clearTextIcon.IsVisible = (bool)newValue;
                view.trailingIcon.IsVisible = false;
                //view.containerFrame.BorderColor = view.errorText.IsVisible ? Color.Red : Color.Black;

                //view.helperText.IsVisible = !view.errorText.IsVisible;
                //view.BorderColor = view.errorText.IsVisible ? Color.Red : view.BorderColor;
                //view.placeholderText.TextColor = view.errorText.IsVisible ? Color.Red : Color.Gray;

                //view.PlaceholderText = view.errorText.IsVisible ? $"{view.PlaceholderText}*" : view.PlaceholderText;

                //if (view.TrailingIcon != null && !view.TrailingIcon.IsEmpty)
                //    view.tempIcon = view.TrailingIcon;

                //view.TrailingIcon = view.errorText.IsVisible
                //    ? ImageSource.FromFile("ic_error.png")
                //    : view.tempIcon;

                //view.trailingIcon.IsVisible = view.errorText.IsVisible;
            }
        );

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
            nameof(IsPassword),
            typeof(bool),
            typeof(FilledEntry),
            default(bool),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.customEntry.IsPassword = (bool)newValue;

                view.passwordIcon.IsVisible = (bool)newValue;
                if(view.passwordIcon.IsVisible)
                    view.clearTextIcon.IsVisible = false;
            }
        );

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            nameof(MaxLength),
            typeof(int),
            typeof(FilledEntry),
            default(int),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.customEntry.MaxLength = (int)newValue;

                view.charCounterText.IsVisible = view.customEntry.MaxLength > 0;

                view.charCounterText.Text = $"0 / {view.MaxLength}";
            }
        );
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(
            nameof(TextColor),
            typeof(Color),
            typeof(FilledEntry),
            Color.Gray,
            BindingMode.TwoWay,
            null,
            (bindable, oldvalor, newvalor) =>
            {
                var control = (FilledEntry)bindable;
                control.customEntry.TextColor = (Color)newvalor;
            }
        );

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(FilledEntry),
            Color.Gray,
            BindingMode.TwoWay,
            null,
            (bindable, oldvalor, newvalor) =>
            {
                var control = (FilledEntry)bindable;
                control.underline.Color = (Color)newvalor;

            }
            );
        public static readonly BindableProperty BorderColorActiveProperty = BindableProperty.Create(
            nameof(BorderColorActive),
            typeof(Color),
            typeof(FilledEntry),
            Color.DarkGray,
            BindingMode.TwoWay
        );

        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(
            nameof(PlaceholderColor),
            typeof(Color),
            typeof(FilledEntry),
            Color.Gray,
            BindingMode.TwoWay, 
            null,
            (bindable, oldvalor, newvalor) =>
            {
                var control = (FilledEntry)bindable;
                control.placeholderText.TextColor = (Color)newvalor;

            }
        );
        public static readonly BindableProperty ActivePlaceholderColorProperty = BindableProperty.Create(
            nameof(ActivePlaceholderColor),
            typeof(Color),
            typeof(FilledEntry),
            Color.DarkGray,
            BindingMode.TwoWay

        );

        public static readonly BindableProperty ReturnCommandProperty = BindableProperty.Create(
            nameof(ReturnCommand),
            typeof(ICommand),
            typeof(FilledEntry),
            default(ICommand),
            BindingMode.OneWay,
            null,
            (bindable, oldValue, newValue) =>
            {
                var view = (FilledEntry)bindable;

                view.customEntry.ReturnCommand = (ICommand)newValue;
            }
        );

        public FilledEntry()
        {
            InitializeComponent();
            //underline.Color = BorderColor;
            //placeholderText.TextColor = PlaceholderColor;
            this.customEntry.Text = this.Text;

            this.customEntry.TextChanged += this.OnCustomEntryTextChanged;

            this.customEntry.Completed += this.OnCustomEntryCompleted;
        }

        // Event Handlers
        public event EventHandler<EventArgs> EntryCompleted;

        public event EventHandler<TextChangedEventArgs> TextChanged;

        // Properties
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        public string HelperText
        {
            get => (string)GetValue(HelperTextProperty);
            set => SetValue(HelperTextProperty, value);
        }

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public ImageSource LeadingIcon
        {
            get => (ImageSource)GetValue(LeadingIconProperty);
            set => SetValue(LeadingIconProperty, value);
        }

        public string TrailingIcon
        {
            get => (string)GetValue(TrailingIconProperty);
            set => SetValue(TrailingIconProperty, value);
        }

        public bool HasError
        {
            get => (bool)GetValue(HasErrorProperty);
            set => SetValue(HasErrorProperty, value);
        }

        public bool ClearText
        {
            get => (bool)GetValue(ClearTextProperty);
            set => SetValue(ClearTextProperty, value);
        }
        public bool IsPassword
        {
            get => (bool)GetValue(IsPasswordProperty);
            set => SetValue(IsPasswordProperty, value);
        }

        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public Color BorderColorActive
        {
            get => (Color)GetValue(BorderColorActiveProperty);
            set => SetValue(BorderColorActiveProperty, value);
        }
        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }
        public Color ActivePlaceholderColor
        {
            get => (Color)GetValue(ActivePlaceholderColorProperty);
            set => SetValue(ActivePlaceholderColorProperty, value);
        }
        public Keyboard Keyboard
        {
            set => this.customEntry.Keyboard = value;
        }

        public ReturnType ReturnType
        {
            set => this.customEntry.ReturnType = value;
        }

        public ICommand ReturnCommand
        {
            get => (ICommand)GetValue(ReturnCommandProperty);
            set => SetValue(ReturnCommandProperty, value);
        }

        private async Task ControlFocused()
        {
            if (string.IsNullOrEmpty(this.customEntry.Text) || this.customEntry.Text.Length > 0)
            {
                this.customEntry.Focus();

               // this.containerFrame.BorderColor = this.HasError ? Color.Red : this.BorderColor;
                this.underline.Color = this.HasError ? Color.Red : this.BorderColorActive;
                this.placeholderText.TextColor = this.HasError ? Color.Red : this.ActivePlaceholderColor;

                int y = DeviceInfo.Platform == DevicePlatform.UWP ? -25 : -20;

                await this.placeholderContainer.TranslateTo(0, y, 110, Easing.Linear);

                this.placeholderContainer.HorizontalOptions = LayoutOptions.Start;
                this.placeholderText.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
            }
            else
                await this.ControlUnfocused();
        }

        // Here we change the border and placeholder text color
        // back to normal and check if there is any text on the entry,
        // if not we launch the animation to place the placeholder
        // back over the entry
        private async Task ControlUnfocused()
        {
            //this.containerFrame.BorderColor = this.HasError ? Color.Red : Color.Black;
            this.placeholderText.TextColor = this.HasError ? Color.Red : this.PlaceholderColor;
            this.underline.Color = this.HasError ? Color.Red : this.BorderColor;

            this.customEntry.Unfocus();

            if (string.IsNullOrEmpty(this.customEntry.Text) || this.customEntry.MaxLength <= 0)
            {
                await this.placeholderContainer.TranslateTo(0, 0, 100, Easing.Linear);

                this.placeholderContainer.HorizontalOptions = LayoutOptions.FillAndExpand;
                this.placeholderText.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
            }
        }

        private void CustomEntryFocused(object sender, FocusEventArgs e)
        {
            if (e.IsFocused)
                MainThread.BeginInvokeOnMainThread(async () => await this.ControlFocused());
        }

        private void CustomEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (!e.IsFocused)
                MainThread.BeginInvokeOnMainThread(async () => await this.ControlUnfocused());
        }

        private void OutlinedMaterialEntryTapped(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () => await this.ControlFocused());
        }

        // Here we change the password type of the entry
        // in order to change the eye icon
        private void PasswordEyeTapped(object sender, EventArgs e)
        {
            this.customEntry.IsPassword = !this.customEntry.IsPassword;
        }

        // Here we set the text by every new char
        // and update the charCounter label
        private void OnCustomEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            this.Text = e.NewTextValue;

            if (this.charCounterText.IsVisible)
                this.charCounterText.Text = $"{this.customEntry.Text.Length} / {this.MaxLength}";

            if (this.Text.Length > 0 && this.ClearText)
                this.clearTextIcon.IsVisible = true;
            else if (this.Text.Length <= 0 && this.ClearText)
                this.clearTextIcon.IsVisible = false;

            this.TextChanged?.Invoke(this, e);
        }

        private void OnCustomEntryCompleted(object sender, EventArgs e)
        {
            this.EntryCompleted?.Invoke(this, EventArgs.Empty);
        }

        private async void ClearTextTapped(object sender, EventArgs e)
        {
            Text = string.Empty;
            await ControlUnfocused();
        }
    }
}