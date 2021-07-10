# 使用Task组件处理异步逻辑

命名空间：`GameFlow`

主要涉及的类是`Task`，`TaskList`，`TaskSet`和`TaskSequence`。

#### 延迟调用

在1秒后打印`"123"`，下面三种方法都能达到一样的效果。

```csharp
Task.Delay(1.0f).OnComplete(()=>print("123")).Play();
```

```csharp
var list = new TaskList(){
    Task.Delay(1.0f),
    ()=>print("123")
}
list.Play();
```

```csharp
var seq = new TaskSequence();
seq.Insert(1.0f, ()=>print("123"));
seq.Play();
```



#### TaskList、TaskSet和TaskSequence的区别

+   `TaskList`是一个接一个的执行，当最后一个`Task`完成后`TaskList`就完成了
+   `TaskSet`是并行执行，顺序不固定，当所有`Task`完成后`TaskSet`就完成了
+   `TaskSequence`可进行绝对时间的插入，同时可当作`TaskList`使用



#### DOTween对象转换成Task

在Unity配置中加入宏定义`DOTWEEN_EX`，所有内置的容器都会自动增加对DOTween对象的支持。

例如你可以：

```csharp
var list = new TaskList(){
    transform.DOMove(Vector3.one, duration: 2f),
    ()=>print("DOTween对象已完成。")
}
```

Note：当你要把DOTween对象加入Task系统里使用时，不要在原生的DOTween对象里注册任何回调，而应该在转换后的`DOTweenTask`对象里去注册。



#### 继承实现自定义Task

```csharp
    public class CustomDelayTask : Task
    {
        public float duration { get; private set; }

        public DelayTask(float duration)
        {
            this.duration = duration;
        }

        protected override void OnPlay()
        {
            StartCoroutine(DelayCoroutine());
        }

        protected IEnumerator DelayCoroutine()
        {
            yield return new WaitForSeconds(duration);
            Complete();		// 主动调Complete通知Task对象完成
        }

        protected override void OnKill()
        {
			// TODO: 被Kill掉会发生什么
        }
    }
```

