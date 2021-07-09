# 创建Dart风格的行为树

下面这个例子展示了一个简单的逻辑，如果看到敌人就攻击，否则就巡逻。

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

