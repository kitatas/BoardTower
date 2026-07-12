# Skill: Unit Testing Implementation

このファイルはClaudeがテストコードを生成する際の**具体的な実装パターン**を定義します。
ルール定義（優先度・命名規則・テンプレート等）は **`.aiassistant/rules/10_unit_testing.md`** を参照してください。

## 1. モッキングパターン（NSubstitute）

```csharp
// 戻り値設定
_repository.GetAsync(Arg.Any<int>()).Returns(UniTask.FromResult(expectedValue));

// 例外設定
_repository.GetAsync(Arg.Any<int>()).Throws(new InvalidOperationException());

// 呼び出し回数の検証
await _repository.Received(1).GetAsync(42);

// 引数の部分一致
await _repository.Received(1).GetAsync(Arg.Is<int>(x => x > 0));
```

## 2. 非同期テストパターン（UniTask）

- `UniTask` を返すメソッドのテストは `.AsTask()` で変換して `await` する
- `CancellationToken` が必要な場合は `CancellationToken.None` を渡す

```csharp
[Test]
public async Task {MethodName}_ShouldCompleteSuccessfully()
{
    // Arrange
    var token = CancellationToken.None;

    // Act
    await _useCase.{MethodName}(token).AsTask();

    // Assert
    // ...
}
```

## 3. R3 / リアクティブプログラミングのテストパターン

- `Subject<T>` を使ってストリームを制御し、手動で `.OnNext()` を発行する
- 購読時の副作用（View更新・状態変化）を `Assert` で検証する
- `TearDown` で `Dispose()` を必ず呼ぶ

```csharp
[Test]
public void OnValueChanged_WhenSubjectEmits_ShouldUpdateState()
{
    // Arrange
    var subject = new Subject<int>();
    subject.Subscribe(v => _entity.SetValue(v));

    // Act
    subject.OnNext(42);

    // Assert
    Assert.That(_entity.Value, Is.EqualTo(42));
}
```

## 4. AAA パターン（必須）

```csharp
[Test]
public void {MethodName}_{Condition}_{ExpectedResult}()
{
    // Arrange（前提条件の構築）
    var input = ...;

    // Act（テスト対象の実行）
    var result = _sut.{MethodName}(input);

    // Assert（結果の検証）
    Assert.That(result, Is.EqualTo(expected), "失敗時の説明メッセージ");
}
```

## 5. MonoBehaviour のテスト制約

- `EditMode` では Unity ライフサイクル（`Start`・`Update` 等）は動作しない
- テスト可能な部分（純粋なロジック・`public` メソッド）のみを対象とする
- `UnityEngine` 依存部分はインターフェース経由でモック化する
- PlayMode テストが必要な場合はユーザーに確認する
