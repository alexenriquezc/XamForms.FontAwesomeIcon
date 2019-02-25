using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamFormsFA
{
    public class FontAwesomeIcon : Label
    {
        #region Bindable Properties
        public static readonly BindableProperty FontTypeProperty = BindableProperty.Create(nameof(FontType), typeof(FontType), typeof(FontAwesomeIcon), FontType.Regular, BindingMode.TwoWay, null, propertyChanged: OnFontTypeChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(FontAwesomeIcon), null, BindingMode.TwoWay, null, propertyChanged: OnCommandChanged);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(FontAwesomeIcon), null, BindingMode.TwoWay, null, propertyChanged: OnCommandParameterChanged);
        #endregion

        #region Properties
        public FontType FontType
        {
            get { return (FontType)GetValue(FontTypeProperty); }
            set { SetValue(FontTypeProperty, value); }
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set { SetValue(CommandParameterProperty, value); }
        }
        #endregion

        #region Ctor
        public FontAwesomeIcon()
        {
            var gesture = new TapGestureRecognizer();
            gesture.Tapped += OnTapped;
            GestureRecognizers.Clear();
            GestureRecognizers.Add(gesture);
        }
        #endregion

        #region Commands
        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set { SetValue(CommandProperty, value); }
        }
        #endregion

        #region Methods
        static void OnFontTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    var fontType = (FontType)bindable.GetValue(FontTypeProperty);
                    bindable.SetValue(FontTypeProperty, fontType);
                    bindable.SetValue(FontFamilyProperty, $"{fontType.Font}.ttf#Font Awesome 5 Free Regular");
                    break;
                case Device.iOS:
                    bindable.SetValue(FontFamilyProperty, "Font Awesome 5 Free");
                    break;
            }
        }

        static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            bindable.SetValue(CommandProperty, (ICommand)newValue);
        }

        static void OnCommandParameterChanged(BindableObject bindable, object oldValue, object newValue)
        {
            bindable.SetValue(CommandParameterProperty, newValue);
        }

        void OnTapped(object _, EventArgs e)
        {
            Command?.Execute(CommandParameter);
        }
        #endregion        
    }

    [TypeConverter(typeof(FontTypeConverter))]
    public struct FontType
    {
        public static readonly FontType Regular = new FontType("fa-regular-400");
        public static readonly FontType Solid = new FontType("fa-solid-900");
        public static readonly FontType Brands = new FontType("fa-brands-400");
        public string Font;

        public FontType(string font)
        {
            Font = font;
        }
    }

    [TypeConversion(typeof(FontType))]
    public sealed class FontTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(FontType);
        }

        public override object ConvertFromInvariantString(string value)
        {
            if (value != null)
            {
                var parts = value.Split('.');
                if (parts.Length > 2 || (parts.Length == 2 && parts[0] != "FontType"))
                    throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(FontType)}");
                value = parts[parts.Length - 1];
                switch (value)
                {
                    case "Regular": return FontType.Regular;
                    case "Solid": return FontType.Solid;
                    case "Brands": return FontType.Brands;
                }

                FieldInfo field = typeof(FontType).GetFields().FirstOrDefault(fi => fi.IsStatic && fi.Name == value);
                if (field != null)
                    return (FontType)field.GetValue(null);
            }

            throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(FontType)}");
        }
    }
}
