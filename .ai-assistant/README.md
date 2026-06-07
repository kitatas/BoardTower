# .ai-assistant

このディレクトリには、AI アシスタントによる開発支援システムの設定とリソースが含まれています。

## 概要

AI Assistant システムは、Unity プロジェクトにおける開発作業を自動化・支援するためのフレームワークです。**Skills**（技術的能力）と **Instructions**（実行手順）を組み合わせることで、様々な開発タスクを効率的に実行できます。

## システム構成

```bash
.ai-assistant/
 ├── README.md # このファイル（システム概要）
 ├── config/ # 設定ファイル
 ├── skills/ # 技術的能力定義
 ├── instructions/ # 実行手順書
 ├── templates/ # コードテンプレート
 ├── workflows/ # ワークフロー定義
 ├── context/ # プロジェクト情報
 ├── examples/ # 実例とサンプル
 ├── tools/ # 支援ツール
 ├── metadata/ # メタデータ
 └── logs/ # 実行ログ
``` 

## 主要機能

### 🧪 テスト自動生成

- 単体テスト、統合テスト、E2Eテストの自動生成
- NUnit、NSubstitute、Unity Test Framework 対応
- モック生成とテストデータ作成

### 🏗️ コード生成

- Entity、UseCase、Presenter クラスの生成
- Clean Architecture パターンの適用
- ボイラープレートコードの削減

### 🔄 リファクタリング支援

- SOLID原則に基づくコード改善
- デザインパターンの適用
- 依存性注入の導入

### 📊 コード分析

- 静的解析とコード品質チェック
- 依存関係の可視化
- パフォーマンス分析

### 📚 ドキュメント生成

- API ドキュメント自動生成
- README、技術仕様書の作成
- アーキテクチャ図の生成

## プロジェクト情報

| 項目 | 値 |
|------|-----|
| プロジェクト名 | BoardTower |
| Unity バージョン | 6000.3.12f1 |
| .NET Framework | 4.7.1 |
| アーキテクチャ | Clean Architecture |
| レンダリング | Universal Render Pipeline (URP) |
| 主要ライブラリ | UniTask, R3, VContainer, MessagePipe |

## 使用方法

### 1. 基本的な流れ
```
mermaid graph LR A[要求] --> B[Skills確認] --> C[Instructions選択] --> D[実行] --> E[結果確認]
``` 

### 2. テスト生成の例

```bash
# 1. 必要なSkillsを確認
AI Assistant: "Unity単体テスト生成のスキルを保有しています"

# 2. Instructionsに従って実行
AI Assistant: "unit-test-generation-instructions.md に従って実行します"

# 3. 結果の確認
Tests/EditMode/ 配下にテストファイルが生成されます
```

### 3. コード生成の例

``` bash
# Entity クラス生成
Input: "UserEntity を生成して"
Output: User データを管理する Entity クラスとテストが生成される
```

## Skills と Instructions の違い

| 項目 | Skills | Instructions |
| --- | --- | --- |
| 定義 | 「何ができるか」 | 「どうやるか」 |
| 内容 | 技術的能力・知識 | 具体的な手順・ルール |
| 例 | "NUnit を使える" | 1. [TestFixture] を付ける<br>2. [SetUp] でセットアップ<br>3. ... |
| 変更頻度 | 低（技術習得時） | 中（手順改善時） |

## ディレクトリ詳細

### 📁 config/

プロジェクト固有の設定ファイル
- `global-config.yml`: 全体設定
- `project-info.yml`: プロジェクト情報
- `environments.yml`: 環境別設定

### 📁 skills/

技術的能力の定義（「何ができるか」）
- 各技術分野別に整理
- 依存関係と前提知識を明記
- 対応可能な範囲を定義

### 📁 instructions/

具体的な実行手順書（「どうやるか」）
- ステップ・バイ・ステップの手順
- 判断基準とエラー処理
- 品質チェック項目

### 📁 templates/

再利用可能なコードテンプレート
- 各種クラステンプレート
- テストテンプレート
- ドキュメントテンプレート

### 📁 workflows/

複数の作業を組み合わせたワークフロー
- CI/CD パイプライン
- コードレビュープロセス
- リリース手順

## バージョン管理

このシステムは semantic versioning に従います：
- **Major**: 破壊的変更（既存の Skills/Instructions との非互換）
- **Minor**: 新機能追加（新しい Skills/Instructions の追加）
- **Patch**: バグ修正、改善

現在のバージョン: **1.0.0**

## 拡張方法

### 新しい Skills の追加

1. `skills/{カテゴリ}/` に新しい `.md` ファイルを作成
2. `metadata/skills-registry.json` に登録
3. 必要に応じて依存関係を設定

### 新しい Instructions の追加

1. `instructions/{カテゴリ}/` に新しい `.md` ファイルを作成
2. `metadata/instructions-registry.json` に登録
3. 対応する Skills との関連付け

### カスタムテンプレートの作成

1. `templates/{カテゴリ}/` に新しいテンプレートを追加
2. Instructions から参照するように更新

## トラブルシューティング

### よくある問題

**Q: テスト生成が失敗する** A:
1. `logs/generation-logs/` でエラー内容を確認
2. 対象クラスの依存関係を確認
3. Unity Test Framework がインストールされているか確認

**Q: 生成されたコードが期待と異なる** A:
1. 使用した Instructions を確認
2. プロジェクト固有設定（`config/project-info.yml`）を確認
3. Templates の内容を確認

**Q: 新しい機能を追加したい** A:
1. まず Skills として能力を定義
2. 次に Instructions として手順を作成
3. 必要に応じて Templates を準備
