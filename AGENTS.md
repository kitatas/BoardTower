# Agent Guidelines

## 共通ルール

以下のファイルに定義された共通ルールをすべて遵守してください。
- `.aiassistant/rules/00_global.md`

## Agent 固有の指示

### 操作上の制約

- `ProjectSettings/` 以下のファイルは原則変更しない
- `Packages/packages-lock.json` は変更しない
- エディタ拡張コードは `Assets/Editor/` に配置し、必ず `#if UNITY_EDITOR` で保護する

### ビルド・CI

- Unity バッチモード実行例:
  `/path/to/Unity -batchmode -quit -projectPath . -executeMethod BuildScript.Build`
