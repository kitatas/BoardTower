# unit-testing-skills

## テクニカルスキル

### 1. テストフレームワーク

- **NUnit**: Unity Test Runner での単体テスト実装
- **Unity Test Framework**: EditMode/PlayMode テストの使い分け
- **Assertion**: 複数パターンの検証方法

### 2. モッキング・スタビング

- **NSubstitute**: インターフェースのモック生成と検証
- **Moq**: 代替モッキングフレームワーク
- **Test Doubles**: Stub, Mock, Spy の使い分け

### 3. 非同期テスト

- **UniTask**: Cysharp.Threading.Tasks での非同期処理テスト
- **CancellationToken**: トークンを使った非同期処理の制御
- **async/await**: 非同期メソッドのテストパターン

### 4. リアクティブプログラミング

- **R3 Observable**: Observableストリームのテスト
- **Subject**: BehaviorSubject, PublishSubject のテスト
- **Subscribe/Dispose**: メモリリーク防止のテストパターン

### 5. 依存性注入テスト

- **Constructor Injection**: コンストラクタ注入のテスト
- **VContainer**: DIコンテナを使ったテスト
- **Interface Segregation**: インターフェース分離でのテスト容易性

### 6. Unity特有のテスト

- **MonoBehaviour**: Unity コンポーネントのテスト手法
- **ScriptableObject**: 設定オブジェクトのテスト
- **UnityEngine**: Unity APIへの依存を最小化したテスト

### 7. アーキテクチャパターン

- **Clean Architecture**: 層別テスト戦略
- **UseCase Pattern**: ビジネスロジックの単体テスト
- **Repository Pattern**: データアクセス層のテスト
- **Presenter Pattern**: プレゼンテーション層のテスト

### 8. テスト設計パターン

- **AAA Pattern**: Arrange-Act-Assert パターン
- **Given-When-Then**: BDD スタイルのテスト記述
- **Test Data Builder**: テストデータ構築パターン
- **Object Mother**: テストオブジェクト生成パターン

### 9. カバレッジ分析

- **Code Coverage**: コードカバレッジの測定と改善
- **Branch Coverage**: 分岐網羅率の向上
- **Path Coverage**: パス網羅のテスト設計

### 10. パフォーマンステスト

- **Benchmark**: パフォーマンス計測
- **Memory Profiling**: メモリ使用量のテスト
- **GC Allocation**: ガベージコレクション負荷の測定
