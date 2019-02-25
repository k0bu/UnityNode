module.exports = {
  root: true,
  env: {
    browser: true,
    node: true
  },
  parserOptions: {
    "extends": "eslint:recommended",//parser: 'babel-eslint',
    ecmaVersion: 6
  },
  extends: [
    "eslint:recommended", //ESLintで基本的なルールチェック(更に細かく個別指定可)
    "plugin:prettier/recommended"  //一番下に書かないとうまく動かない場合も
  ],
  plugins: [],
  // ここにカスタムルールを追加します。
  rules: {
    "prettier/prettier": [
      "error",
      {
        "singleQuote": true, //シングルクォーテーションのフォーマット 参考）https://prettier.io/docs/en/options.html#quotes
        "semi": true //セミコロンのフォーマット 参考）https://prettier.io/docs/en/options.html#semicolons
      }
    ],
  }
}