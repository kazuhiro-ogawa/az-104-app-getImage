# 問題に使用する画像を取得するためのAzure Functions

AZ-104 TrainingApp(https://github.com/kazuhiro-ogawa/az-104-react-app.git)
で使用する、画像取得用のAPIです。  
Azure FunctionsでC#を用いて作成しています。Azure Blob Storageから画像を取得します。  
HTTPリクエストがトリガーで、レスポンスで取得した画像を返します。

## システム構成図
![getimage](https://github.com/kazuhiro-ogawa/az-104-app-getImage/assets/105719508/1f920bc0-0770-41e1-80b3-28afd507d4cd)

