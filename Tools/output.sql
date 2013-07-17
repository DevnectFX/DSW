
-- 사용자정보
DROP TABLE UserInfo;
CREATE TABLE UserInfo (
     USER_ID varchar(16) NOT NULL     -- 사용자아이디
   , NAME varchar(16) NOT NULL     -- 이름
   , DEL_YN char(1) NOT NULL DEFAULT 'N'     -- 삭제유무
   , GROUP_ID varchar(16)     -- 그룹아이디
   , PRIMARY KEY(USER_ID)
   , FOREIGN KEY(GROUP_ID) REFERENCES GroupInfo(GROUP_ID)
);
-- 사용자인증
DROP TABLE UserCert;
CREATE TABLE UserCert (
     USER_ID varchar(16) NOT NULL     -- 사용자아이디
   , PASSWD varchar(256) NOT NULL     -- 암호
   , PRIMARY KEY(USER_ID)
   , FOREIGN KEY(USER_ID) REFERENCES UserInfo(USER_ID)
);
-- 사용자추가정보
DROP TABLE UserExtraInfo;
CREATE TABLE UserExtraInfo (
     USER_ID varchar(16) NOT NULL     -- 사용자아이디
   , PRIMARY KEY(USER_ID)
   , FOREIGN KEY(USER_ID) REFERENCES UserInfo(USER_ID)
);
-- 사용자상세정보
DROP TABLE UserDetailInfo;
CREATE TABLE UserDetailInfo (
     USER_ID varchar(16) NOT NULL     -- 사용자아이디
   , BIRTH_DT datetime     -- 출생일
   , PRIMARY KEY(USER_ID)
   , FOREIGN KEY(USER_ID) REFERENCES UserInfo(USER_ID)
);
-- 사용자로그인이력
DROP TABLE UserLoginHistory;
CREATE TABLE UserLoginHistory (
     USER_ID varchar(16) NOT NULL     -- 사용자아이디
   , SEQ integer NOT NULL DEFAULT '1'     -- 순번
   , LOGIN_DT datetime NOT NULL     -- 로그인시각
   , PRIMARY KEY(USER_ID, SEQ)
   , FOREIGN KEY(USER_ID) REFERENCES UserInfo(USER_ID)
);
-- 사용자변경이력
DROP TABLE UserChangeHistory;
CREATE TABLE UserChangeHistory (
     USER_ID varchar(16) NOT NULL     -- 사용자아이디
   , SEQ integer NOT NULL DEFAULT '1'     -- 순번
   , CHNG_ID varchar(16) NOT NULL     -- 변경아이디
   , CHNG_DT datetime NOT NULL     -- 변경시각
   , PRIMARY KEY(USER_ID, SEQ)
   , FOREIGN KEY(USER_ID) REFERENCES UserInfo(USER_ID)
);
-- 사용자권한
DROP TABLE UserAuth;
CREATE TABLE UserAuth (
     USER_ID varchar(16) NOT NULL     -- 사용자아이디
   , AUTH_TYPE varchar(8) NOT NULL     -- 권한유형
   , PRIMARY KEY(USER_ID)
   , FOREIGN KEY(USER_ID) REFERENCES UserInfo(USER_ID)
   , FOREIGN KEY(AUTH_TYPE) REFERENCES AuthTypeInfo(AUTH_TYPE)
);
-- 그룹정보
DROP TABLE GroupInfo;
CREATE TABLE GroupInfo (
     GROUP_ID varchar(16) NOT NULL     -- 그룹아이디
   , PARENT_GROUP_ID varchar(16)     -- 상위그룹아이디
   , NAME varchar(128) NOT NULL     -- 이름
   , SORT_ORDER int NOT NULL DEFAULT '0'     -- 정렬순서
   , USE_YN char(1) NOT NULL DEFAULT 'Y'     -- 사용유무
   , PRIMARY KEY(GROUP_ID)
);
-- 그룹권한
DROP TABLE GroupAuth;
CREATE TABLE GroupAuth (
     GROUP_ID varchar(16) NOT NULL     -- 그룹아이디
   , AUTH_TYPE varchar(8) NOT NULL     -- 권한유형
   , PRIMARY KEY(GROUP_ID)
   , FOREIGN KEY(GROUP_ID) REFERENCES GroupInfo(GROUP_ID)
   , FOREIGN KEY(AUTH_TYPE) REFERENCES AuthTypeInfo(AUTH_TYPE)
);
-- 그룹추가정보
DROP TABLE GroupExtraInfo;
CREATE TABLE GroupExtraInfo (
     GROUP_ID varchar(16) NOT NULL     -- 그룹아이디
   , PRIMARY KEY(GROUP_ID)
   , FOREIGN KEY(GROUP_ID) REFERENCES GroupInfo(GROUP_ID)
);
-- 그룹상세정보
DROP TABLE GroupDetailInfo;
CREATE TABLE GroupDetailInfo (
     GROUP_ID varchar(16) NOT NULL     -- 그룹아이디
   , PRIMARY KEY(GROUP_ID)
   , FOREIGN KEY(GROUP_ID) REFERENCES GroupInfo(GROUP_ID)
);
-- 그룹변경이력
DROP TABLE GroupChangeHistory;
CREATE TABLE GroupChangeHistory (
     GROUP_ID varchar(16) NOT NULL     -- 그룹아이디
   , SEQ integer NOT NULL DEFAULT '1'     -- 순번
   , CHNG_ID varchar(16) NOT NULL     -- 변경아이디
   , CHNG_DT datetime NOT NULL     -- 변경시각
   , PRIMARY KEY(GROUP_ID, SEQ)
   , FOREIGN KEY(GROUP_ID) REFERENCES GroupInfo(GROUP_ID)
);
-- 권한유형정보
DROP TABLE AuthTypeInfo;
CREATE TABLE AuthTypeInfo (
     AUTH_TYPE varchar(8) NOT NULL     -- 권한유형
   , PARENT_AUTH_TYPE varchar(8)     -- 상위권한유형
   , AUTH_TXT varchar(32) NOT NULL     -- 권한명
   , DESC_TXT varchar(256)     -- 설명
   , PRIMARY KEY(AUTH_TYPE)
);
-- 메뉴정보
DROP TABLE MenuInfo;
CREATE TABLE MenuInfo (
     MENU_ID varchar(16) NOT NULL     -- 메뉴아이디
   , PARENT_MENU_ID varchar(16)     -- 상위메뉴아이디
   , MENU_TXT varchar(32) NOT NULL     -- 메뉴명
   , MENU_DESC_TXT varchar(256)     -- 메뉴설명
   , SORT_ORDER int NOT NULL DEFAULT '0'     -- 정렬순서
   , MENU_PATH varchar(256)     -- 메뉴경로
   , EXTRA_INFO varchar(256)     -- 추가정보
   , USE_YN char(1) NOT NULL DEFAULT 'Y'     -- 사용유무
   , CHNG_ID varchar(16)     -- 변경아이디
   , CHNG_DT datetime     -- 변경시각
   , CREATE_ID varchar(16) NOT NULL     -- 생성아이디
   , CREATE_DT datetime NOT NULL     -- 생성시각
   , PRIMARY KEY(MENU_ID)
);
-- 메뉴추가정보
DROP TABLE MenuExtraInfo;
CREATE TABLE MenuExtraInfo (
     MENU_ID varchar(16) NOT NULL     -- 메뉴아이디
   , PRIMARY KEY(MENU_ID)
   , FOREIGN KEY(MENU_ID) REFERENCES MenuInfo(MENU_ID)
);
-- 사용자메뉴
DROP TABLE UserMenu;
CREATE TABLE UserMenu (
     USER_ID varchar(16) NOT NULL     -- 사용자아이디
   , MENU_ID varchar(16) NOT NULL     -- 메뉴아이디
   , CREATE_AUTH char(1) NOT NULL DEFAULT 'N'     -- 생성권한
   , READ_AUTH char(1) NOT NULL DEFAULT 'N'     -- 읽기권한
   , UPDATE_AUTH char(1) NOT NULL DEFAULT 'N'     -- 갱신권한
   , DEL_AUTH char(1) NOT NULL DEFAULT 'N'     -- 삭제권한
   , HIDE_YN char(1) NOT NULL DEFAULT 'N'     -- 숨김유무
   , REG_ID varchar(16) NOT NULL     -- 등록아이디
   , REG_DT datetime NOT NULL     -- 등록시각
   , PRIMARY KEY(USER_ID, MENU_ID)
   , FOREIGN KEY(USER_ID) REFERENCES UserInfo(USER_ID)
   , FOREIGN KEY(MENU_ID) REFERENCES MenuInfo(MENU_ID)
);
-- 그룹메뉴
DROP TABLE GroupMenu;
CREATE TABLE GroupMenu (
     GROUP_ID varchar(16) NOT NULL     -- 그룹아이디
   , MENU_ID varchar(16) NOT NULL     -- 메뉴아이디
   , CREATE_AUTH char(1) NOT NULL DEFAULT 'N'     -- 생성권한
   , READ_AUTH char(1) NOT NULL DEFAULT 'N'     -- 읽기권한
   , UPDATE_AUTH char(1) NOT NULL DEFAULT 'N'     -- 갱신권한
   , DEL_AUTH char(1) NOT NULL DEFAULT 'N'     -- 삭제권한
   , HIDE_YN char(1) NOT NULL DEFAULT 'N'     -- 숨김유무
   , REG_ID varchar(16) NOT NULL     -- 등록아이디
   , REG_DT datetime NOT NULL     -- 등록시각
   , PRIMARY KEY(GROUP_ID, MENU_ID)
   , FOREIGN KEY(GROUP_ID) REFERENCES GroupInfo(GROUP_ID)
   , FOREIGN KEY(MENU_ID) REFERENCES MenuInfo(MENU_ID)
);
-- 공통코드
DROP TABLE CommonCode;
CREATE TABLE CommonCode (
     CODE_ID varchar(8) NOT NULL     -- 코드아이디
   , PARENT_CODE_ID varchar(8)     -- 상위코드아이디
   , CODE_TXT varchar(32) NOT NULL     -- 코드명
   , CODE_DESC_TXT varchar(256)     -- 코드설명
   , SORT_ORDER int NOT NULL DEFAULT '0'     -- 정렬순서
   , USE_YN char(1) NOT NULL DEFAULT 'Y'     -- 사용유무
   , PRIMARY KEY(CODE_ID)
);
-- 메시지
DROP TABLE Message;
CREATE TABLE Message (
     MSG_ID varchar(16) NOT NULL     -- 메시지아이디
   , MSG varchar(256) NOT NULL     -- 메시지
   , CHNG_ID varchar(16)     -- 변경아이디
   , CHNG_DT datetime     -- 변경시각
   , CREATE_ID varchar(16) NOT NULL     -- 생성아이디
   , CREATE_DT datetime NOT NULL     -- 생성시각
   , PRIMARY KEY(MSG_ID)
);
-- 첨부파일정보
DROP TABLE AttachFileInfo;
CREATE TABLE AttachFileInfo (
     ATTACH_FILE_SEQ integer NOT NULL DEFAULT '1'     -- 첨부파일순번
   , FILE_UUID varchar(40) NOT NULL     -- 파일UUID
   , FILE_PATH varchar(256) NOT NULL     -- 파일경로
   , FILE_TXT varchar(256) NOT NULL     -- 파일명
   , FILE_SIZE integer NOT NULL     -- 파일크기
   , CREATE_ID varchar(16) NOT NULL     -- 생성아이디
   , CREATE_DT datetime NOT NULL     -- 생성시각
   , PRIMARY KEY(ATTACH_FILE_SEQ, FILE_UUID)
);