# 简易的事件系统

- 用于C#开发中的事件系统，支持Unity，导入即可使用

## 使用方法
创建一个自定义事件
```C#
public YourCustomEvent : Event{
    //定义你需要传入参数时候的属性
}
```
创建一个监听器
```C#
public YourListener : EventListener{
    //使用特性声明这个方法是需要监听的方法，参数输入为事件的优先级，默认是Normal
    //Lowest,
    //Low,
    //Normal,
    //High,
    //Highest,
    //上面是事件的优先级顺序
    [EventListen(EventPriority.Highest)]
    public void Fuctions(YourCustomEvent e){
        //函数名称任意
        //函数参数必须是继承于Event的数据类
        //处理逻辑
    }
}
```

监听的函数格式必须是
```C#
    //这里参数可以空 默认为Normal
    [EventListen()]
    public void Fuctions(YourCustomEvent e){
        //函数名称任意
        //函数参数必须是继承于Event的数据类
        //处理逻辑
    }

```

在合适的地方创建监听器管理器
```C#
//实例化监听器管理器
EventListenerManager eventListenerManager = new EventListenerManager();
//实例化监听器
YourListener listener = new YourListener();
//添加监听器
eventListenerManager.AddListener(listener);
````

在合适地方通过监听器管理器调用监听事件
```C#
//再合适时机 调用自定义时间
YourCustomEvent event = new YourCustomEvent();
eventListenerManager.FireListenerEvent(event);
```

删除监听器
```C#
eventListenerManager.RemoveListener(llistener);
```

## 类说明
### Event
- 用于创建自定义事件的基类，所有需要自定义事件都需要继承Event

### EventListenerManager
- 监听器管理器，用于管理监听器，如果需要使用事件管理器，需要实例化一个监听器管理器，并且需要添加监听器

### EventListener
- 监听器，事件函数处理，需要继承这个类，并且在合适的地方，进行实例化，通过EventListenerManager添加至管理中

### EventListen
- 特性，用于识别监听器需要监听的函数，如果函数继承了EventListener但是没有添加特性说明，这个函数并不会被监听到

### EventHandler
- 事件管理器，用于处理事件和函数指针之间关系，这个实际运用并不涉及

