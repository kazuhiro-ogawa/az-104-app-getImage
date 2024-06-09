# 問題に使用する画像を取得するためのAzure Functions

AZ-104 TrainingApp(https://github.com/kazuhiro-ogawa/az-104-react-app.git)
で使用する、画像取得用のAPIです。Azure FunctionsでC#を用いて作成しています。Azure Blob Storageから画像を取得します。  
HTTPリクエストがトリガーで、レスポンスで取得した画像を返します。

## システム構成図
![システム構成図_azurefunctions_getimage](https://github.com/kazuhiro-ogawa/az-104-app-getImage/assets/105719508/7f8b9370-c1e6-4216-ac56-c3cf65170d36)
