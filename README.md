itch.io link (post the work)



# Custom-Visual-Effect

# Table of Contents
- [[1] Func Overview](#1-Func-Overview)
  - [Map Overview](#Map-Overview)
  - [Inventory Overview](#Inventory-Overview)
  - [Camp Overview](#Camp-Overview)
  - [Shop Overview](#Shop-Overview)
- [[2] PopGainEffect](#2-PopGainEffect)
- [[3] FogEffect_SceneChange](#3-FogEffectSceneChange)
- [[4] Posted Link](#4-PostedLink)
- [[0] Acknowledgement](#0-Acknowledgement)


# [1] Func Overview

## Map Overview

![image](https://github.com/ln32/Project_RoglikeGame/assets/94381505/bc9be3b1-7cea-4538-a04d-66dbf847ba7d)

### 구현 기능 목록

1. 기록된 맵 데이터에 따라 노드를 진행시키며, 적절한 조건에 따른 전체 노드 생성합니다.
2. 노드와 노드 사이를 이어주는 길 점선 노드들 구현합니다.
3. 멥 데이터에 따른 적절한 아이콘 표시합니다.
4. 터치 이벤트 발생 시, 해당 노드의 정보를 읽고 우측 하단에 띄우고 해당 노드로 가는 길에 해당하는 Sprite들을 가져와서 점등합니다. 
5. 맵 이동 시 입력 제한 및 맵 이동 연출과 동시에 다음 씬 로드 진행합니다.
6. 진행 후 맵 데이터 갱신 후, 갱신된 맵에 알맞은 데이터들을 생성합니다.

## Inventory Overview

![image](https://github.com/ln32/Project_RoglikeGame/assets/94381505/7e8fb4ff-bf5f-4e16-8997-0fce4af0af91)

### 구현 기능 목록

1. Inventory 진입 시 Charactor Data를 읽은 후, GUI에 출력합니다.
2. 기록된 Item Data 들을 읽으며 적절한 칸에 적절한 Item들이 배치되도록 합니다.
3. Slot 간 Drag Drop 상호작용을 구현했습니다.  ( 이동, 강화, 장착, 소모 )
4. 조건을 따져가며 각각의 상태에 따른 Effect를 출력합니다. ( 적합, 부적합, Focus )
5. Item Fouce 시, 해당 아이템의 데이터를 출력합니다.
6. Item을 Charactor Equip Slot에 장착 시 정보 동기화 후 알맞은 GUI로 갱신합니다.

## Camp  Overview

![image](https://github.com/ln32/Project_RoglikeGame/assets/94381505/7bdb1fc9-1486-4043-a07a-d5c18190a44b)

### 구현 기능 목록

1. Camp 진입 시 Item Data를 읽은 후, 알맞은 Slot에 배치합니다.
2. Ingredient Slot에 음식 아이템을 Drag Drop 하면 배치 상호작용이 됩니다.
3. 각 Slot은 인접한 Slot과 상호작용하며 시너지가 발생하고 아래 GUI에 표시됩니다.
4. 모든칸에 배치가 된 후 func 버튼으로 Cook를 진행하고, 시너지에 따른 요리가 나옵니다.
5. 한 번 배치한 재료는 다른 Slot에 배치 하게 되면 기존 배치는 취소되며, 배치된 Item은  Inventory에 하얗게 칠해집니다.

## Shop Overview

![image](https://github.com/ln32/Project_RoglikeGame/assets/94381505/12050e4f-9593-4041-999f-d900e2583dbb)

### 구현 기능 목록

1. Shop 진입 시 Item Data를 읽은 후, 알맞은 Inventory Slot에 배치합니다.
2. Goods를 Inventory Slot에 Drag Drop 시, 구입 상호작용 발생
3. Inventory Item을 Sell Table에 Drag Drop 시, 판매 상호작용 발생
4. Item을 Info Box에 Drag Drop 시, Focus 상호작용 발생
5. 구입 및 판매 시 Item Data 저장하고 Return 버튼 클릭 시, 해당 데이터를 업로드 합니다.


# [2] Structure

Adding

# [3] Related Implementation

Adding

# [4] Posted Link
 https://jaehoon602.itch.io/map-inven-camp-shop-720p
 
last updated - 24.07.05.


# [0] Acknowledgement
- (https://github.com/dev-ujin/awesome-readme-template/blob/master/res-readme/README-KO.md#features)
