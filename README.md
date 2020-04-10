# GameFlow工具包（测试版）

为同时兼任策划和程序的游戏制作人打造的开发工具包！

A Designer-Oriented Flow-Control Toolkit for Game Interactions in Unity.

<br>

**GameFlow-Tookit** 是一款用于制作Unity游戏的开源工具包，通过它可以轻松创建和管理游戏机制的交互。gf出自2018年秋季探索（Autumn Quest），作为GameDevKit核心组件，经过多个项目的实践积累，精化并逐步完善。特点如下：

+ 快速实验玩法
+ 轻量、极简的API
+ 初级到进阶适用

## 安装与配置

([最新版本](https://github.com/blueloveTH/gameflow-toolkit/releases/tag/latest_release))  ([所有版本](https://github.com/blueloveTH/gameflow-toolkit/releases))

通过upm安装：

1. Windows/Package Manager
2. Add package from disk...
3. 选择package.json



运行环境：Unity 2018.4及以上



## 快速开始

#### TaskList

```c#
//当空格键被按下后，输出Hello, world!
TaskList list = new TaskList(){
    Task.WaitUntil(() => Input.GetKeyDown(KeyCode.Space)),
    () => Debug.Log("Hello, world!"),
}

list.onComplete += () => Debug.Log("The list is completed.");
list.Play();
```



#### FlowDiagram

```c#
//使用函数式API创建状态图
public enum Colors{
    Black, white
}

//定义图和节点
FlowDiagram fd = FlowDiagram.CreateByEnum<Colors>();

//定义回调
fd[Colors.Black].onEnter += (x) => spRenderer.color = Color.black;
fd[Colors.white].onEnter += (x) => spRenderer.color = Color.white;

//初始化
fd.Enter(Colors.Black);
```



#### InteractionUnit

```c#

```



#### BehaviourTree

```c#

```



## 获取支持

如果您在使用中遇到问题，可通过如下方式联系我：

+ QQ：2551900781

+ Email： blueloveTH@foxmail.com



## 致谢

