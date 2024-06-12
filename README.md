# RsLogix500Helper  
本系統的功能為剛學習RsLogix 500的輔助小程式，協助初學者理解階梯圖的意義。  
*  RsLogix 500 用於編寫Allen-Bradley PLC (MicroLogix 系列 \ SLC 500 系列) 的程式碼

正常RsLogix500的程式畫面  
![image](https://github.com/cs8694530/RsLogix500Helper/assets/19258631/529cf873-c0b9-4f1e-9a16-4ed9ea192d66)

由於沒接觸過這方面的工程師對於階梯圖以及其圖示不易理解，因此做了本程式來協助入門  
![image](https://github.com/cs8694530/RsLogix500Helper/assets/19258631/1ddf87a6-6dab-4d3c-8cdc-e1771e59e866)

主畫面中的「問號符號」有說明如何儲存RsLogix 500的檔案  
*  由於在編寫的時候配合需求只針對常用的進行翻譯，若有遺漏可以在程式碼中自行補充

![image](https://github.com/cs8694530/RsLogix500Helper/assets/19258631/a0d0d0a7-9c43-4759-aed5-23c0ca3badf1)

選擇檔案後，即可進行翻譯  
*  右側按鈕可以在原始碼與翻譯的文本進行交互比對
*  右側選單可以進行LAD的過濾 (LAD 即為副程式的概念)
*  若測試資料比較龐大，在翻譯的時候有可能會卡住一下子 (沒有針對Loading寫多執行緒)
![image](https://github.com/cs8694530/RsLogix500Helper/assets/19258631/506e441f-d4f9-47ed-a822-590d40cb39f3)
![image](https://github.com/cs8694530/RsLogix500Helper/assets/19258631/36cee5e7-ddb7-490a-9fd7-fc089e26ded7)

