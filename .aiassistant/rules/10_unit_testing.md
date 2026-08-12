---
apply: always
---

# Rule: Unit Test Implementation

このファイルはユニットテストを生成する際に従うべきルールを定義します。
テスト対象クラスを指示された場合、このファイルの内容に従いテストコードを生成してください。

具体的な実装パターン（NSubstitute・R3・UniTask等）は **`.aiassistant/skills/unit_testing.md`** を参照してください。

## 依存関係の特定

- プロジェクト内の他ファイルを勝手に探索して依存関係を解析することは禁止します（トークン消費と誤認を防ぐため）。
- 必ず最初に **`.aiassistant/rules/05_architecture.md`** を開いて内容を確認してください。そのクラスが属するレイヤーに従って、即座にテストの構成を決定してください。

## テスト対象分類

### 優先度 High

- Entity クラス (Data/Entity 配下)
- UseCase クラス (Domain/UseCase 配下)
- ValueObject クラス (Application 配下)

### 優先度 Medium

- Ports クラス (Domain/Ports 配下)
- Presenter クラス (Presentation/Presenter 配下)
- Facade クラス (Presentation/Facade 配下)

### 優先度 Low

- View クラス (Presentation/View 配下) ※MonoBehaviour除く
- MonoBehaviour継承クラス ※テスト可能部分のみ

## ファイル命名・配置規則

| 対象クラス | テストファイル名 | 配置パス |
|---|---|---|
| `RetryCountEntity.cs` | `RetryCountEntityTests.cs` | `Tests/EditMode/Data/Entity/` |
| `{ClassName}.cs` | `{ClassName}Tests.cs` | `Tests/EditMode/{元と同じ相対パス}/` |

## テストクラス共通テンプレート

```csharp
[TestFixture]
public class {ClassName}Tests
{
    private {ClassName} _{fieldName};

    [SetUp]
    public void SetUp()
    {
        // インスタンス生成・依存関係のモック設定
    }

    [TearDown]
    public void TearDown()
    {
        // リソースクリーンアップ
    }
}
```

## クラス種別ごとのテスト生成指示

### Entity クラス

下記のテストパターンをすべて実装すること。

```csharp
// 1. コンストラクタ正常系
[Test]
public void Constructor_WithValidParameters_ShouldInitializeCorrectly()

// 2. publicメソッド 正常系
[Test]
public void {MethodName}_With{Condition}_Should{ExpectedResult}()

// 3. publicメソッド 異常系
[Test]
public void {MethodName}_WithInvalid{Parameter}_ShouldThrow{Exception}()

// 4. 境界値テスト
[TestCase(minValue)]
[TestCase(maxValue)]
[TestCase(zeroValue)]
public void {MethodName}_WithBoundaryValues_ShouldHandleCorrectly(type value)

// 5. プロパティ検証
[Test]
public void {PropertyName}_WhenSet_ShouldReturnCorrectValue()
```

### UseCase クラス

SetUpでは **Entity は実インスタンスで生成し**、Repository・Ports はモック化してUseCaseを構築すること。

```csharp
[TestFixture]
public class {UseCaseName}Tests
{
    private {UseCaseName} _useCase;
    private {EntityType} _entity;
    private {PortsType} _ports;
    private {RepositoryType} _repository;

    [SetUp]
    public void SetUp()
    {
        _entity = new {EntityType}();
        _ports = Substitute.For<{PortsType}>();
        _repository = Substitute.For<{RepositoryType}>();
        _useCase = new {UseCaseName}(_entity, _ports, _repository);
    }
}
```

下記のテストパターンをすべて実装すること。

```csharp
// 1. 初期化テスト
[Test]
public async Task InitAsync_ShouldInitializeCorrectly()

// 2. ビジネスロジック 正常系
[Test]
public async Task {MethodName}_WithValid{Parameter}_Should{ExpectedBehavior}()

// 3. 例外処理テスト
[Test]
public void {MethodName}_WithInvalid{Parameter}_ShouldThrow{Exception}()

// 4. 依存メソッド呼び出し検証
[Test]
public async Task {MethodName}_ShouldCall{DependencyMethod}()
{
    // Act
    await _useCase.{MethodName}();

    // Assert
    await _ports.Received(1).{ExpectedMethod}(Arg.Any<{Type}>());
}
```

### ValueObject クラス

下記のテストパターンをすべて実装すること。

```csharp
// 1. 無効値でのコンストラクタ異常系
[TestCase("")]
[TestCase(" ")]
[TestCase(null)]
public void Constructor_WithInvalidValue_ShouldThrowException(string invalidValue)

// 2. 範囲外値での境界値テスト
[TestCase(minValue - 1)]
[TestCase(maxValue + 1)]
public void Constructor_WithOutOfRangeValue_ShouldThrowException(int outOfRange)

// 3. 正常値での生成テスト
[TestCase(validValue1)]
[TestCase(validValue2)]
public void Constructor_WithValidValue_ShouldSetValueCorrectly(type validValue)

// 4. ファクトリメソッドテスト（存在する場合）
[Test]
public void Create_ShouldReturnValidInstance()
```

### Ports クラス

下記のテストパターンをすべて実装すること。

```csharp
// 1. 依存注入コンストラクタテスト
[Test]
public void Constructor_WithValidDependencies_ShouldInitializeCorrectly()

// 2. Publishメソッドテスト
[Test]
public async Task Publish{Type}Async_ShouldCallPublisher()
{
    // Arrange
    var data = new {Type}();
    var token = CancellationToken.None;

    // Act
    await _ports.Publish{Type}Async(data, token);

    // Assert
    await _publisher.Received(1).PublishAsync(data, token);
}

// 3. Subscribeメソッドテスト（存在する場合）
[Test]
public void Subscribe{Type}_ShouldRegisterHandler()
```

## 品質チェックリスト

生成後に以下をすべて満たしているか確認すること。
- [ ] すべての `public` メソッドにテストが存在する
- [ ] 正常系・異常系・境界値のテストが存在する
- [ ] 依存関係のモック検証が適切に行われている
- [ ] 各テストが他のテストに依存していない（独立性）
- [ ] テストメソッド名が `{対象}_{条件}_{期待結果}` 形式になっている
- [ ] すべてのテストが Arrange-Act-Assert パターンに従っている
- [ ] `IDisposable` なリソースが `TearDown` で破棄されている

## テスト更新ルール

| 変更種別 | 対応アクション |
| --- | --- |
| 元クラスに新 `public` メソッド追加 | 対応テストメソッドを追加 |
| メソッドシグネチャ変更 | テストを修正 |
| ビジネスロジック変更 | テストケースを見直し |
| 依存関係変更 | `SetUp` のモック設定を更新 |

## 制約・ベストプラクティス

### 制約事項

- `MonoBehaviour` は EditMode では限定的なテストのみ実施する
- `UnityEngine` 依存部分はモック化またはスタブに置き換える
- 非同期処理は `UniTask` を使用している場合、`.AsTask()` で変換して `await` する

### ベストプラクティス

- **1テスト = 1検証**。複数の事項を1つのテストで検証しない
- テストデータはテストメソッド内で生成し、外部状態に依存しない
- 外部リソース（ファイル・ネットワーク等）への依存を避ける
- テスト実行順序に依存しない設計にする
- `Assert` のメッセージ引数に失敗時の説明を含める
