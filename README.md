# GameFlow工具包（测试版）

为同时兼任策划和程序的游戏制作人打造的开发工具包！

A Designer-Oriented Flow-Control Toolkit for Game Mechanisms in Unity.

<br>

**GameFlow-Tookit** 是一款用于制作Unity游戏的开源工具包，通过它可以轻松创建和管理游戏机制的交互。gf出自2018年秋季探索（Autumn Quest），作为GameDevKit核心组件，经过多个项目的实践积累，精化并逐步完善。特点如下：

+ 快速实验玩法
+ 轻量、极简的API
+ 初级到进阶适用

## 安装与配置

([latest release](https://github.com/blueloveTH/gameflow-toolkit/releases/tag/latest_release))  ([all releases](https://github.com/blueloveTH/gameflow-toolkit/releases))





## 快速开始

```c#
//当玩家按下鼠标左键后输出Hello, world!
TaskList list = new TaskList(){
    Task.WaitUntil(() => Input.GetMouseButtonDown(0)),
    () => Debug.Log("Hello, world!"),
}

list.Play();
```



