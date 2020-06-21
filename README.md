# sbid-ava
🔮内生安全建模工具，基于.Net Core 3.0的Avalonia跨平台桌面应用。

## 简要展示
### 类图
包括自定义数据类型、进程模板、InitialKnowledge、公理、SafetyProperty、SecurityProperty。

![类图展示](https://raw.githubusercontent.com/LauZyHou/pic/master/sbid-ava/ClassDiagram.png)

### 状态机图
用于进程模板的行为建模。

![状态机图展示](https://raw.githubusercontent.com/LauZyHou/pic/master/sbid-ava/StateMachine.png)

### 拓扑图
todo

### 攻击树
用于威胁建模。

![攻击树展示](https://raw.githubusercontent.com/LauZyHou/pic/master/sbid-ava/AttackTree.png)


### CTL语法树
todo

### 顺序图
用于面向对象的情景建模。

![顺序图展示](https://raw.githubusercontent.com/LauZyHou/pic/master/sbid-ava/SequenceDiagram.png)

## 发布
对于64位Windows/Linux/OSX可以直接在Visual Studio里Release模式下发布，其它操作系统可以使用命令行。

例如，要发布32位Windows7的版本，进入`sbid`目录，并执行：
```
dotnet publish -r win7-x86 -c release -f netcoreapp3.0 /p:PublishTrimmed=true /p:PublishReadyToRun=true
```
