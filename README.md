# Unity_FlappyBird [앵무탈출]
* **개발 인원**: 1명

* **개발 환경**: Visual Studio, C#, Unity 2020.3.29f1, GPGS 10.14, GoogleMobileAds 7.2.0, Firebase 9.2.0

* **개발 기간**: 2022.10 ~ 2022.12 (약 2달)

* **지원 플랫폼**: Android

* **프로젝트 개요**: Unity 엔진으로 제작한 2D Flappy Bird 게임. <br>새가 바닥에 닿지 않도록 화면을 터치하여 장애물을 피하고 점수를 올리는 플레이 방식. <br>파이어베이스를 연동하여 다른 플레이어들과 점수 경쟁을 할 수 있도록 랭킹 시스템을 구현.

* **핵심 기능**
   - **Firebase + GPGS + Google AdMob 연동**
   <br>3개의 플러그인을 연동하여 구글 로그인 기능, 랭킹 시스템, 광고 출력 기능을 구현하고 구글 플레이스토어에 출시.
   - **성능 관리를 위한 오브젝트 풀링 기법**
   <br>장애물 오브젝트가 등장할 때마다 새롭게 생성하고 파괴하게 되면 GC로 인한 프레임 드랍이 발생할 수 있기 때문에 이를 방지하기 위해 오브젝트 풀링 기법을 사용함.
   - **OnTriggerEnter를 이용한 게임오버와 점수획득**
   <br>유니티 내장함수인 OnTriggerEnter를 이용하여 장애물에 부딪혔을 때 게임오버와 장애물 사이를 통과했을 때 점수를 획득하는 기능을 구현함.

# 포트폴리오 기술서
https://drive.google.com/file/d/1opbyA_tCJLZ_If6Nd635PN4jGMOyyvgY/view?usp=sharing
