# GameFlow工具包

A Lightweight Flow-Control Toolkit for Gameplay.

**GameFlow-Toolkit** 是轻量级的、用于游戏流程控制的Unity工具包，出自2018年秋季探索（Autumn Quest）。其包含如下四大特性，整合了一套流程处理的最佳实践。

+   Task组件处理异步逻辑
+   Signal机制管理碰撞交互
+   异步状态机
+   Dart风格的行为树

## 安装与配置

([最新版本](https://github.com/blueloveTH/gameflow-toolkit/releases/latest))

1. Open the package manager.
2. Click the plus icon on the top left.
3. Select "Add package from git URL".
4. Paste "https://github.com/blueloveTH/gameflow-toolkit.git".

运行环境：Unity 2019.4及以上



## 教程页

[https://gameflow-toolkit.readthedocs.io/zh_CN/latest/api_references/task/](https://gameflow-toolkit.readthedocs.io/zh_CN/latest/api_references/task/)



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
        // TODO: 遇到地刺
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

//定义异步转换任务
fm[Colors.Black].taskOnEnter += Task.Delay(1.0f);

//切换状态
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
