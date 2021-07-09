# 创建异步状态机

异步状态机，是融合了Task组件的状态机，非常适合游戏里的过渡场景。

假设有状态A、B、C，两两之间可以相互转换，转换时先播放一个动画，播放完成后转换成功。

我们可以定义一个枚举来表示状态：

```csharp
public enum State{
    A, B, C
}
```

然后，使用`FlowMachine<T>`来为这个枚举生成异步状态机，

```csharp
var fm = new FlowMachine<State>();
```

定义每个状态在进入和退出时应该做什么，

```csharp
fm[State.A].onEnter += ()=>print("进入状态A。");
fm[State.B].onExit += ()=>print("退出状态B。");
```

定义异步动作，进入状态A必须先等待1秒。

```
fm[State.A].taskOnEnter += Task.Delay(1.0f);
```

把动画封装成`Task`对象，并添加到状态中，就可以很容易地实现这种逻辑。





