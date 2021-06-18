# WarFactory——战车工厂
 **幻影坦克+无影坦克全面手机APP解决方案（手动滑稽）**  
 
在这个APP中，你可以用你的手机生成一张幻影坦克图片（目前只支持灰度），也可以生成或现形一张无影坦克图片  

发布版APK文件附在GitHub项目根目录，方便在网盘挂掉后获取下载更新。  
你也可以在百度网盘下载已经打包好的APK文件: https://pan.baidu.com/s/1B9Aw7J_cKA6W5GwNodRzPA 提取码：pbd6   
百度贴吧帖子: https://tieba.baidu.com/p/7308060718 https://tieba.baidu.com/p/7308042884  

**使用方法详见APP内部**  

这是一个Xamarin.Form解决方案，但目前仅编写了Android项目。  
主要的幻影/无影坦克的核心编解码类位于WarFactory项目下FactoryFunc目录中  
项目根目录附上了一个我自己写的用于生成Xamarin.Android中应用程序图标的C# Console程序。  

By: f_Endman

**更新日志：**  
Version 1.0  
2021-04-18  
    软件发布，基本实现幻影坦克和无影坦克的编解码，尚存在一些Bug  

Version 1.1  
2021-04-24  
    做了许多调整，包括但不仅限于  
    1.调整UI  
        幻影坦克工厂将亮度系数以及阈值的调整移入了一个新界面  
        查看大图界面添加了滚动功能，可以浏览较长的图片  
    2.无影坦克现形现在可以选择多张图片进行解码了(直接全选相册，High到不行)，非图片文件增加了图标显示，大部分图片无需保存即可直接查看，其他文件可保存后即可点击打开  
    3.优化了算法，现在能够解码压缩度为5的坦克图了(之前不知道还有这个)  
    4.修复了若干Bug，包括但不仅限于：  
        第一次打开应用直接解无影坦克而导致闪退的问题  
        生成幻影坦克时的闪退问题  
        有损坏的图进行解码会导致闪退的问题  
        …………  

Version 1.2  
2021-05-16  
    修复了图片保存后相册中找不到的Bug  

Version 1.3  
2021-06-18  
    添加了对左上角的水印的自定义功能  
    暂时解决了证书问题  
    增加了微信赞赏界面，因此出于隐私问题隐藏了该GitHub储存库的大部分文件，只保留了关键算法和UI部分，从今往后该解决方案将不再能直接打开，需要专业人士自行移植到自己的解决方案里，还望体谅  