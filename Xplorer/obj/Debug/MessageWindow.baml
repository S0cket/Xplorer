   M S B A M L   `   `   `  ????B  >Xplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:XplorerXplorer  5          Xplorer.MessageWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      'localclr-namespace:Xplorer     5        ? Title$  MessageWindow??5      $	??200??6   $	??500??6&    ? WindowStyle$
 None=?62    ? 	Resources 5
       ?? MergedDictionaries ?? Source?? 5       5      ?? 5      $ /Styles/WindowStyle.xaml??6   %    5      ??5      ?? 5       ?? 
TargetType" ?6
   .?5      ??  ?? Property ?@J?SnapsToDevicePixels ?? Value . 6+   ?? 5       ? ??   WindowBackground# [  6"   ?? 5      '  !System.Windows.Shell.WindowChrome ?@ WindowChrome 5        5         CaptionHeight$ 30Q?6   	   ResizeBorderThickness$	 5q?6'   
   CornerRadius$
 0??6A      GlassFrameThickness$ 0q?6R   5      ?? 5       ? ?? 5      ?? 5       ?? 
TargetType" ?6   .R?5      ?? 
???????._?5      ?? #??z??6   $??1q?69   ._?5    	   ? .+?5!   
   ??   WindowChromeBackground#??[ 6   $??30??6F   $	??Top=?6R   ._?5"      ? .+?5#      ?? $
??Left=?6   $??Center=?61     TitleForeground#??[ 6L   #??z  5$      $??10,0,0,0q?6/   ?? 5%      $??Right=?6   $??Center=?63    ?? Orientation$ 
Horizontal=?6N   .+?5&      ?? -   5          ChromeButton#??[ 5&      $??40??6:   $??30??6E      IsHitTestVisibleInChrome .   WindowCloseIcon#???  5'      6:   5(      5)      5*      ?? 5+   
   .u?5,      ?? $??0,30,0,0q?6   5-      5.   
   5/   	   50   	   51      53      54      .??57      ? .+?58      ?? $ Vertical=?6   $??Center=?6&   $??Center=?6A   .+?59      ?? -   5         ??Name$ MainText??59      
???????$??Center=?64   $
??Wrap=?6Q   $??10q?6e   ?? 5:      -   5        #??[ 5:      $	??200??62   $??40??6>   
???????.??6q   ОК6u   5;      5<      5=      