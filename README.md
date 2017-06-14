﻿# わらドット

ドット絵エディターをこれから作るところ。

# 実行に必要な準備

## NLua ライブラリの入手
取得元 https://github.com/NLua/NLua

Win64 版をクリック
NLua.Win64.zip をダウンロード
その中の net45 フォルダーの中に

- KeraLua.dll
- lua52.dll
- NLua.dll

がある。

## 参照の追加
Visual Studio 2017 のソリューション エクスプローラーから、[参照] - [参照の追加] で
参照マネーシャーを出す。

[参照]ボタンをクリック。
NLua.dll と、 KeraLua.dll を選択。

## 64bitに設定
ツールバーの[Any CPU]をクリックして 構成マネージャーを出す。
[アクティブ ソリューション プラットフォーム]をクリックし[新規作成]を選択。

名前を x64 にする。
※ これで [プロジェクト] - [WaraDot のプロパティ] - [ビルド] - [プラットフォームターゲット] が x64 になる。
※ 32bit/64bitは混在できないので。

## ファイルの移動
lua52.dll を Source/WaraDot/bin/x64/Debug に入れてください。

# 作業用フォルダーの作成
bin/x64/Debug の下（.exeがあるところ）に Work フォルダーを丸ごと移動させてください。

# ビジュアル・エディターのための設定

出典:「デザイン画面でコントロールが表示されなくなる」 http://scallop-shell27.rssing.com/chan-14601842/latest.php

```
データが失われる可能性を防ぐため、デザイナーの読み込み前に以下のエラーを解決する必要があります。
```

といったエラーが出た場合の対応方法。

- Source フォルダーを右クリック、[プロパティ] - [全般] - [詳細設定] - [このフォルダー内のファイルに対し、プロパティだけでなくコンテンツにもインデックスを付ける]のチェックを外す。
- リビルドする。

それでもダメな場合

- Form1 をビジュアル・エディターで編集する場合だけ、[x64] ではなく [Any CPU] に変更する。


