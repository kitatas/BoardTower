# unit-test-generation-instructions

## フェーズ1: プロジェクト分析

### 1.1 ファイル構造分析

```bash
# プロジェクト構造をスキャン
1. Assets/ 配下の .cs ファイルを列挙
2. 名前空間とクラス名を抽出
3. 継承関係とインターフェース実装を特定
4. 依存関係グラフを作成
```

### 1.2 テスト対象分類

#### 優先度 High

- Entity クラス (Data/Entity 配下)
- UseCase クラス (Domain/UseCase 配下)
- ValueObject クラス (Application 配下)

#### 優先度 Medium

- Ports クラス (Domain/Ports 配下)
- Presenter クラス (Presentation/Presenter 配下)
- Facade クラス (Presentation/Facade 配下)

#### 優先度 Low

- View クラス (Presentation/View 配下) ※MonoBehaviour除く
- MonoBehaviour継承クラス ※テスト可能部分のみ

### 1.3 依存関係マッピング

```csharp
// 依存関係分析のパターン
1. コンストラクタ注入の特定
2. プロパティ注入の特定  
3. メソッド注入の特定
4. 静的依存の特定
5. Unity依存の特定
```

## フェーズ2: テストクラス生成

### 2.1 Entity クラステスト生成

#### ファイル命名規則

```csharp
// 元ファイル: RetryCountEntity.cs
// テストファイル: RetryCountEntityTests.cs
// 配置場所: Tests/EditMode/Data/Entity/
```

#### テンプレート構造

``` csharp
[TestFixture]
public class {ClassName}Tests
{
    private {ClassName} _{fieldName};
    
    [SetUp]
    public void SetUp()
    {
        // インスタンス生成
        // 依存関係のモック設定
    }
    
    [TearDown] 
    public void TearDown()
    {
        // リソースクリーンアップ
    }
    
    // テストメソッド群
}
```

#### 必須テストパターン

``` csharp
// 1. コンストラクタテスト
[Test]
public void Constructor_WithValidParameters_ShouldInitializeCorrectly()

// 2. 各publicメソッドの正常系テスト
[Test]  
public void {MethodName}_With{Condition}_Should{ExpectedResult}()

// 3. 各publicメソッドの異常系テスト
[Test]
public void {MethodName}_WithInvalid{Parameter}_ShouldThrow{Exception}()

// 4. 境界値テスト
[TestCase(minValue)]
[TestCase(maxValue)]
[TestCase(zeroValue)]
public void {MethodName}_WithBoundaryValues_ShouldHandle{Correctly}(type value)

// 5. プロパティテスト
[Test]
public void {PropertyName}_WhenSet_ShouldReturnCorrectValue()
```

### 2.2 UseCase クラステスト生成

#### テンプレート構造

``` csharp
[TestFixture]
public class {UseCaseName}Tests
{
    private {UseCaseName} _useCase;
    private {EntityType} _entity;
    private {PortsType} _ports;
    
    [SetUp]
    public void SetUp()
    {
        _entity = new {EntityType}();
        _ports = Substitute.For<{PortsType}>();
        _useCase = new {UseCaseName}(_entity, _ports);
    }
}
```

#### 必須テストパターン

``` csharp
// 1. 初期化テスト
[Test]
public async Task InitAsync_ShouldInitializeCorrectly()

// 2. 各ビジネスロジックメソッドのテスト
[Test]
public async Task {MethodName}_WithValid{Parameter}_Should{ExpectedBehavior}()

// 3. 例外処理テスト
[Test] 
public void {MethodName}_WithInvalid{Parameter}_ShouldThrow{Exception}()

// 4. 依存関係の呼び出し検証
[Test]
public async Task {MethodName}_ShouldCall{DependencyMethod}()
{
    // Act
    await _useCase.{MethodName}();
    
    // Assert
    await _ports.Received(1).{ExpectedMethod}(Arg.Any<{Type}>());
}
```

### 2.3 ValueObject クラステスト生成

#### 必須テストパターン

``` csharp
// 1. コンストラクタ検証テスト
[TestCase("")]
[TestCase(" ")]
[TestCase(null)]
public void Constructor_WithInvalidValue_ShouldThrowException(string invalidValue)

// 2. 境界値テスト
[TestCase(minValue - 1)]
[TestCase(maxValue + 1)]  
public void Constructor_WithOutOfRangeValue_ShouldThrowException(int outOfRange)

// 3. 正常値テスト
[TestCase(validValue1)]
[TestCase(validValue2)]
public void Constructor_WithValidValue_ShouldSetValueCorrectly(type validValue)

// 4. ファクトリメソッドテスト
[Test]
public void Create_ShouldReturnValidInstance()
```

### 2.4 Ports クラステスト生成

#### 必須テストパターン

``` csharp
// 1. 依存関係注入テスト
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

// 3. Subscribeメソッドテスト（該当する場合）
[Test]
public void Subscribe{Type}_ShouldRegisterHandler()
```

## フェーズ3: テスト実行・検証

### 3.1 テスト実行

``` bash
# Unity Test Runner での実行
1. Window > General > Test Runner を開く
2. EditMode タブを選択
3. Run All または個別実行
4. 結果確認
```

### 3.2 カバレッジ確認

``` csharp
// Code Coverage パッケージ使用
1. Window > Analysis > Code Coverage を開く
2. Enable Code Coverage を有効化
3. テスト実行後にカバレッジ確認
4. 目標: 80% 以上のカバレッジ
```

### 3.3 品質チェック

## チェック項目
- [ ] すべてのpublicメソッドにテストが存在
- [ ] 正常系・異常系・境界値のテストが存在  
- [ ] 依存関係のモック検証が適切
- [ ] テストの独立性が保たれている
- [ ] テストメソッド名が適切
- [ ] Arrange-Act-Assert パターンが守られている

## フェーズ4: メンテナンス

### 4.1 テスト更新ルール

1. 元クラスに新メソッド追加 → 対応テスト追加
2. メソッドシグネチャ変更 → テスト修正
3. ビジネスロジック変更 → テストケース見直し
4. 依存関係変更 → モック設定更新

## 注意事項

### 制約事項

- MonoBehaviour は Unity環境が必要なためEditModeでは限定的
- UnityEngine 依存部分は適切にモック化が必要
- 非同期処理は UniTask.ToUniTask() でラップして検証

### ベストプラクティス
- 1つのテストで1つの事項のみ検証
- テストデータはテストメソッド内で生成
- 外部リソースへの依存を避ける
- テスト実行順序に依存しない設計
- 意味のあるアサーションメッセージを含める
