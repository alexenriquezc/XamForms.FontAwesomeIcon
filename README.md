# XamForms.FontAwesomeIcon
You can use fontawesome 5.7 in xamarin forms projects.

for the correct behavior you should copy the fonts and past in each platform (Android/Assets, iOS/Resources)

[wintellect example](https://www.wintellect.com/using-fontawesome5-xamarin-forms/)

Example

#XAML
```XAML
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"            
             xmlns:fa="clr-namespace:XamFormsFA;assembly=XamFormsFA"
             xmlns:icon="clr-namespace:XamFormsFA.Helpers;assembly=XamFormsFA"
             x:Class="XamarinTest.MainPage">

    <StackLayout Padding="10">
        <fa:FontAwesomeIcon Text="{Static icon:Icon.AngleDown}" FontType="Solid" FontSize="50"/>
        <fa:FontAwesomeIcon Text="&#xf170;" FontType="Brands" FontSize="50"/>
    </StackLayout>
</ContentPage>
```
**If you want use more font aweome icons you should enter the unicode like this and specify the fontType, you can use Solid and brand type but Regular type has a weird behavior.**



