   M S B A M L   `   `   `  ????B  >Xplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:XplorerXplorer  5   	       Xplorer.TaskWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Xplorer     5   	     ? Title$  
TaskWindow??5   	   $	??200??6   $	??400??6)    ? WindowStyle$
 None=?65    ? 	Resources 5
       ?? MergedDictionaries ?? Source   Key ?? Color?? 5       5      ?? 5      $ /Styles/WindowStyle.xaml??6   ?? 5      ?? 5      BorderColor  $ Wheat??6+   5      %    5      ??5      ?? 5       ?? 
TargetType" ?6
   .?5      ??  ?? Property ?@J?SnapsToDevicePixels ?? Value . 6+   ?? 5       ? ??  WindowBackground# [ 6"   ?? 5      '  !System.Windows.Shell.WindowChrome ?@ WindowChrome 5        5      	   CaptionHeight$	 30Q?6   
   ResizeBorderThickness$
 5q?6'      GlassFrameThickness$ 0q?6A      CornerRadius$ 0??6Y   5      ?? 5       ? ?? 5      ?? 5       ?? 
TargetType" ?6   .R?5       ?? 
???????._?5!      ?? #??z??6   $??1q?69   ._?5"   	   ? .+?5#   
   ?? $??30??6     WindowChromeBackground#??[ 6   $	??Top=?6R   ._?5$      ? .+?5%      ?? $
??Left=?6   $??Center=?61     TitleForeground#??[ 6L   #??z  5&      $??10,0,0,0q?6/   ?? 5'      $??Right=?6   $??Center=?63    ?? Orientation$ 
Horizontal=?6N   .+?5(      ?? -   5          ChromeButton#??[ 5(      $??40??6:   $??30??6E      IsHitTestVisibleInChrome .   WindowCloseIcon#???  5)      6:   5*      5+      5,      ?? 5-   
   .u?5.      ?? $??0,30,0,0q?6   5/      50   
   51   	   52   	   53      55      56      .??59      ? $??5q?6   .+?5:      ?? $ Vertical=?6   $??Center=?6&   .+?5;      ?? 
???????$
??Wrap=?6"   $??Center=?66   $??0,0,0,20q?6S   ?? 5<      $?	Файл ??6
    ?? 5=      -   5         ??Name$ FileName??5=   
    ?? 5>      $G?A уже существует. Что хотите сделать???6
   5?      ?? 5@      -   5         ??Name$ CheckAll??5@      $??Center=?6    
???????$??5q?6P   .??6[   Для всех6e   ?? 5A      $ 
Horizontal=?6   $??Center=?6)   .+?5B      ?? -   5        #??[ 5B      $	??100??63   $??40??6?   $??5q?5C      
???????.??6:   Заменить6D   ?? 5D      -   5        #??[ 5D      $	??100??63   $??40??6?   $??5q?5E      
???????.??67   Пропустить6C   5G      5H      5I      5J      