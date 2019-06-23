https://www.cnblogs.com/Irving/p/4607104.html

//微信跟用户没有关系类接口采用了OAUTH2 【客户端模式(Client Credentials Grant)】，而跟用户有关系的接口，采用OAuth2.0服务端【授权码模式(Authorization Code)】来获得用户的openid；另外需要注意的一点就是需要在开发者中心页配置授权回调域名，域名必须与设置的域名在同一个域下。

基于OWIN WebAPI 使用OAuth授权服务【客户端模式(Client Credentials Grant)】

采用Client Credentials方式，即应用公钥、密钥方式获取Access Token，适用于任何类型应用，但通过它所获取的Access Token只能用于访问与用户无关的Open API，并且需要开发者提前向开放平台申请，成功对接后方能使用。认证服务器不提供像用户数据这样的重要资源，仅仅是有限的只读资源或者一些开放的 API。例如使用了第三方的静态文件服务，如Google Storage或Amazon S3。这样，你的应用需要通过外部API调用并以应用本身而不是单个用户的身份来读取或修改这些资源。这样的场景就很适合使用客户端证书授权,通过此授权方式获取Access Token仅可访问平台授权类的接口。

比如获取App首页最新闻列表,由于这个数据与用户无关，所以不涉及用户登录与授权,但又不想任何人都可以调用这个WebAPI，这样场景就适用[例:比如微信公众平台授权]。