# GameFlow工具包

A Lightweight Flow-Control Toolkit for Gameplay.

<br>

**GameFlow-Toolkit** 是一个轻量级的、用于实现游戏流程控制的Unity工具包，出自2018年秋季探索（Autumn Quest），其包含四大核心组件。

## 安装与配置

([最新版本](https://github.com/blueloveTH/gameflow-toolkit/releases/latest))

通过upm安装：

1. Windows/Package Manager
2. Add package from disk...
3. 选择package.json



运行环境：Unity 2019.4及以上



## 快速开始

#### 使用Task组件处理异步逻辑

```c#
//例：等待空格键按下
TaskList list = new TaskList(){
    Task.WaitUntil(() => Input.GetKeyDown(KeyCode.Space)),
    () => Debug.Log("SPACE key down."),
}

list.onComplete += () => Debug.Log("The list is completed.");
list.Play();
```

#### 使用Signal机制管理碰撞交互

```c#
//例：发送者
public class PlayerSignal : InteractiveBehaviour{
    void OnTriggerEnter2D(Collider2D c2d){
        Emit(Signal("player/hit"), c2d.gameObject);
    }
}

//例：接收者
public class SpikeTrigger : InteractiveBehaviour{
    [SlotMethod("player/hit")]
    void OnSignal(){
        //signal received
    }
}
```


#### 创建异步状态机

```c#
public enum Colors{
    Black, white
}

//从枚举生成异步状态图
FlowMachine fm = new FlowMachine<Colors>();

//定义回调
fm[Colors.Black].onEnter += (x) => spRenderer.color = Color.black;
fm[Colors.white].onEnter += (x) => spRenderer.color = Color.white;

//初始化
fm.Enter(Colors.Black);
```


#### 创建Dart风格的行为树

```csharp
BehaviourTree tree = new BehaviourTree(
	owner: this,
	child: new Sequencer(
		children: new BehaviourNode[]
			{
				new Conditional(
					predicate: () => EnemyDetected(),
					child: new Lambda(() => print("Attack!"))
				),
				new Lambda(()=>print("Wander."))
			}
		)
	);

tree.Play();
```



## 获取支持

如果您在使用中遇到问题，可通过如下方式联系作者：

+ QQ：2551900781

+ Email： blueloveTH@foxmail.com
