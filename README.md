# GameFlow工具包

A Lightweight Flow-Control Toolkit for Gameplay.

<br>

**GameFlow-Toolkit** 是一款用于制作Unity游戏的工具包，通过它可以轻松创建和管理游戏机制的交互。gf出自2018年秋季探索（Autumn Quest），经过多个项目的实践积累，精化并逐步完善。主要特性如下：

+ 快速实现流程控制代码
+ 轻量、极简的API

## 安装与配置

([最新版本](https://github.com/blueloveTH/gameflow-toolkit/releases/latest))

通过upm安装：

1. Windows/Package Manager
2. Add package from disk...
3. 选择package.json



运行环境：Unity 2019.4及以上



## 快速开始

#### TaskList

```c#
//例：等待空格键按下
TaskList list = new TaskList(){
    Task.WaitUntil(() => Input.GetKeyDown(KeyCode.Space)),
    () => Debug.Log("SPACE key down."),
}

list.onComplete += () => Debug.Log("The list is completed.");
list.Play();
```



#### FlowMachine

```c#
//例：使用函数式API创建状态图
public enum Colors{
    Black, white
}

//定义图和节点
FlowMachine fd = FlowMachine.FromEnum<Colors>();

//定义回调
fd[Colors.Black].onEnter += (x) => spRenderer.color = Color.black;
fd[Colors.white].onEnter += (x) => spRenderer.color = Color.white;

//初始化
fd.Enter(Colors.Black);
```



#### InteractiveBehaviour

```c#
//例：发送者
public class EmitterNode : InteractiveBehaviour{
    void OnTriggerEnter2D(Collider2D c2d){
        Emit(Signal("on_touch"), c2d.gameObject);
    }
}

//例：接收者
public class SlotNode : InteractiveBehaviour{
    [SlotFunction("on_touch")]
    void OnSignal(){
        //signal received
    }
}
```



## 获取支持

如果您在使用中遇到问题，可通过如下方式联系作者：

+ QQ：2551900781

+ Email： blueloveTH@foxmail.com
