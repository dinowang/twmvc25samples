# ASP.NET MVC A/B Testing

A/B testing aka split testing, bucket testing

## Information

twMVC#25 | ASP.NET MVC A/B Testing 的眉眉角角

- [Event Information](https://mvc.tw/event/2016/12/24)
- [Slide](https://mvc.tw/event/2016/12/24)

## Sample highlights

### View A/B Testing

Using ASP.NET MVC Display Modes approach, see [~/App_Start/DisplayModeConfig.cs](twmvc25/App_Start/DisplayModeConfig.cs)

### Action A/B Testing

Using customized IActionInvoker approach, see [~/Controller/CtrlTestController.cs](twmvc25/Controllers/CtrlTestController.cs)

- Behavior 1  
  ![Alt text](http://g.gravizo.com/svg?
  digraph G {
  pad = .5;
  node [shape=rect];
  "Request" [label="Request http://localhost/CtrlTest"];
  "Request" -> "CtrlTestController";
  "CtrlTestController" -> "Index" [color=blue, label="A"];
  "CtrlTestController" -> "IndexPreview" [color=red, label="B"];
  "Index" -> "Index.cshtml" [color=blue];
  "IndexPreview" -> "Index.cshtml" [color=red];
  "Index.cshtml" -> "Response to browser";
  }
  )
- Behavior 2  
  ![Alt text](http://g.gravizo.com/svg?
  digraph G {
  pad = .5;
  node [shape=rect];
  "Request" [label="Request http://localhost/CtrlTest"];
  "Request" -> "CtrlTestController";
  "CtrlTestController" -> "Index" [color=blue, label="A"];
  "CtrlTestController" -> "IndexPreview" [color=red, label="B"];
  "Index" -> "Index.cshtml" [color=blue];
  "IndexPreview" -> "IndexPreview.cshtml" [color=red];
  "Index.cshtml" -> "Response to browser" [color=blue];
  "IndexPreview.cshtml" -> "Response to browser" [color=red];
  }
  )
