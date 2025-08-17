const urls = {
  category: {
    list: "category",
    detail: (categoryId: number) => `category/${categoryId}`,
    add: "category",
    update: (categoryId: number) => `category/${categoryId}`,
  },
  order: {
    list: "order",
    cart: "order/cart",
    add: "order",
  },
  product: {
    list: "product",
  },
  user: {
    login: "user/login",
    logout: "user/logout",
    refreshTokens: "user/refresh-tokens",
  },
};

export default urls;
