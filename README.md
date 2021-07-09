# GameFlow工具包

A Lightweight Flow-Control Toolkit for Gameplay.

<br>

**GameFlow-Toolkit** 是一个轻量级的、用于实现游戏流程控制的Unity工具包，出自2018年秋季探索（Autumn Quest），其包含四大核心组件。异步任务，信号槽，状态机和行为树。

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
    [SlotMethod("on_touch")]
    void OnSignal(){
        //signal received
    }
}
```


#### FlowMachine

```c#
//例：使用函数式API创建状态机
public enum Colors{
    Black, white
}

//定义图和节点
FlowMachine fm = new FlowMachine<Colors>();

//定义回调
fm[Colors.Black].onEnter += (x) => spRenderer.color = Color.black;
fm[Colors.white].onEnter += (x) => spRenderer.color = Color.white;

//初始化
fm.Enter(Colors.Black);
```


#### BehaviourTree

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
