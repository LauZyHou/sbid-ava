# sbid-ava
🔮内生安全建模工具，基于.Net Core 3.0的Avalonia跨平台桌面应用。

## 简要展示
### 类图
包括自定义数据类型、进程模板、InitialKnowledge、公理、SafetyProperty、SecurityProperty。



### 状态机图
用于进程模板的行为建模。



### 拓扑图
用于网络拓扑关系建模。



### 攻击树
用于威胁建模。



### CTL语法树
用于从CTL抽象语法树自动生成CTL公式。



### 顺序图
用于面向对象的情景建模。



### 发布
发布前，模式先改成`Release`。

#### 跨平台发布成单个文件
以下操作在`sbid`项目下。

- 配置：Release | Any CPU
- 目标框架：netcoreapp3.0
- 部署模式：独立
- 目标运行时：win-x64或osx-x64或linux-x64
- 文件发布选项：只勾选“生成单个文件”

发布后会有两个文件，手动将`backxml`和`resource`目录复制过去即可。

#### Windows安装包发布
以下操作在`setup`项目下。

- 右键，点重新生成，即可生成`.msi`和`.exe`的安装包。

