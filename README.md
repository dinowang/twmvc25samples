# ASP.NET MVC A/B Testing

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
  node [shape=rect];
  "Request" [label="Request\nhttp://localhost/CtrlTest"];
  "Request" -> "CtrlTestController";
  "CtrlTestController" -> "Index" [style=dotted];
  "CtrlTestController" -> "IndexPreview" [color=red][label="instead method"];
  "Index" -> "Index.cshtml" [style=dotted];
  "IndexPreview" -> "Index.cshtml" [style=red];
  "Index.cshtml" -> "Response to browser";
  }
  )
- Behavior 2  
  ![Alt text](http://g.gravizo.com/svg?
  digraph G {
  node [shape=rect];
  "Request http://localhost/CtrlTest" -> "CtrlTestController";
  "CtrlTestController" -> "Index" [style=dotted];
  "CtrlTestController" -> "IndexPreview" [color=red][label="instead action"];
  "Index" -> "Index.cshtml" [style=dotted];
  "IndexPreview" -> "IndexPreview.cshtml" [color=red];
  "Index.cshtml" -> "Response to browser" [style=dotted];
  "IndexPreview.cshtml" -> "Response to browser" [color=red];
  }
  )
