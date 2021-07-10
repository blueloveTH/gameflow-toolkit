# 使用信号机制管理碰撞交互

命名空间：`GameFlow`

信号机制可使你的碰撞逻辑更具可维护和可扩展性，创建一个子物体，并继承`InteractiveBehaviour`来编写信号/接收器组件。

#### 发送者

如下图所示，在角色Player下创建一个子物体Emitter，并为其加上碰撞触发器，用于发射一个`player/hit`的信号，表示玩家与某对象产生了碰撞。

+   Player
    +   Emitter (PlayerSignal.cs)

```csharp
public class PlayerSignal : InteractiveBehaviour{
    void OnTriggerEnter2D(Collider2D c2d){
        Emit(Signal("player/hit"), c2d.gameObject);
    }
}
```

这样就定义了一个信号，Player只需要把这信号发出，而不关心谁是接收者。

#### 接收者

我们同样继承`InteractiveBehaviour`来定义，任何物体接收到`player/hit`这个信号时应该做什么。

最简单的办法是使用`[SlotMethod("player/hit")]`标记一个方法，如下：

```csharp
public class SpikeTrigger : InteractiveBehaviour{
    [SlotMethod("player/hit")]
    void OnSignal(Signal sig){
        print("遇到地刺！")
    }
}
```

信号推荐的命名方法是`xx/xx`，因为`SlotMethod`可以开启正则表达式，从而进行模糊匹配。

第二种定义信号接收者的方法是使用`AddSlot`，它允许显式过滤信号，更加灵活。



#### 调试

使用信号机制的另一大好处，是可以跟踪每一次交互。

+   将`Signal`对象的`debugMode`置为`true`，使系统跟踪这个信号，输出日志。
+   将`InteractiveBehaviour`对象的`debugMode`置为`true`，使系统跟踪所有到达该对象的信号，输出日志。





