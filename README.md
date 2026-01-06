# 安裝教學
### 前置作業
遊戲使用 Unity 6000.0.64f1 開發，務必先透過 Unity Hub 下載該版本
### 編譯專案
1. 透過 Unity Hub 以指定版本開啟專案
2. 進入 File > Build Profiles
<img width="1016" height="443" alt="image" src="https://github.com/user-attachments/assets/2efd53e6-7d51-4378-b8ab-56cce8a570b5" />
3. 切換目標平台至 Android
<img width="928" height="668" alt="image" src="https://github.com/user-attachments/assets/7f4a8def-ec0f-4c5a-86a3-0d765237e19b" />
4. 進入 Player Settings，並確認選擇 Android 的 Tab
<img width="725" height="82" alt="image" src="https://github.com/user-attachments/assets/56da0bbc-2f5e-40c0-a06d-693a825483b9" />
<img width="1231" height="48" alt="image" src="https://github.com/user-attachments/assets/fce784fc-64f4-4c4f-a5c1-9d93e5fd3bd8" />
5. 找到 Other Settings，查看這些設定
* Minimum API Level 請設為 Android 9.0
* Target API Level 請設為 Android 13.0
* Package Name 可視需求更改，要與你開發的其他 App 不同，以免無法安裝
<img width="1216" height="173" alt="image" src="https://github.com/user-attachments/assets/1aa5d212-d718-4d0e-a790-d14f7a4515d2" />
6. （選用）若你要分發 apk，沒有簽章的 apk 有可能無法在其他裝置上執行，可以透過 Publishing Settings 下建立一個 Keystore 來為 apk 簽章
<img width="1181" height="109" alt="image" src="https://github.com/user-attachments/assets/bb150293-d4cf-4d0d-8981-78f3a0ca8313" />
7. 進入 Meta > Tools > Project Setup Tool
<img width="830" height="510" alt="image" src="https://github.com/user-attachments/assets/8731e602-a27c-4167-a942-fe691ad1051e" />
8. 確認你的專案沒有錯誤，若有，請透過 Fix All 與 Apply All 來嘗試修正
<img width="1381" height="674" alt="image" src="https://github.com/user-attachments/assets/69cfc20c-36cd-4a17-adf7-19a9f4dc6361" />
9. 進入 File > Build Profiles，按下 Build ，選好 destination directory 後可以產生 apk 在指定的 directory
<img width="789" height="172" alt="image" src="https://github.com/user-attachments/assets/7a2786e5-c48d-469c-8a19-d09a5b8dd7b2" />

# 操作說明

左手柄搖桿：前後左右移動 <br/>
右手柄：持有小地圖供檢視遊玩狀況、所在位置與鬼的位置

# 架構圖

<img width="948" height="712" alt="image" src="https://github.com/user-attachments/assets/6930c1fa-4879-4b54-bc62-05338812c378" />


