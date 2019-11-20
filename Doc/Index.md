# ManicureTools <color="lime">(LTS)</color>
## 玲珑工具集
## 简介 
/{<color="orange">▶详细说明</color>
### 玲珑插件
一个仅由很少的资源(几个脚本,几张贴图等)组成的完整功能的插件。我称之为玲珑插件(小工具)。

这种类型的插件对其他插件没有依赖，可以导入任何项目中而不需要考虑引用依赖资源复用等等问题。
同时，这类型的插件也非常容易修改。不像一些完整的解决方案。这类型插件，完全可以根据项目需求定制修改。
但是这类插件数量众多管理不便，如果将他们都打包在一起显然是浪费的。而一个个单独拆分又是牛刀杀鸡。
因此这个工具要解决的就是，这种类型插件的管理和集成问题。
## 管理
所有的插件依然按照UPM格式制作。但是不在单独成立package目录和配置。拆分到ManicureTools插件中对应文件夹下。
注意文档的拆分。

## 集成
通过SeanLibManager导入时 单独勾选需要保留的玲珑插件。
按照原有流程导入后，删除其他不需要的插件。同时可选择 移动到项目文件中。
这个过程比较复杂切需要兼容很多情况(暂定)
/}
## 玲珑汇

***

## CustomMipmap
导入自定义Mipmap
>CustomMipmap/CustomMipmap

## UVPreview
在inspectorPreview中浏览模型UV。
>UVPreview/Index

##TortoiseSVN
Unity编辑器中SVN操作

>TortoiseSVN/README

## ReorderableList 

Unity内置可排序列表开源版
(https://github.com/cfoulston/Unity-Reorderable-List.git)

## UnityBuiltinResouces

移除Unity内置资源。最终打包时减少包体重复资源。

>UnityBuiltinResouces/Index

## LocalCache

本地缓存。支持各种模式的缓存处理。

## Downloader

文件下载器。极简化的文件下载工具。
支持同步/异步 断点续传。

## AudioControler

简单音频控制

## CustomCoroutine

自定义协程 描述