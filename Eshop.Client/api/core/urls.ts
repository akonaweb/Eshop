const urls = {
  category: {
    list: "category",
    detail: (categoryId: number) => `category/${categoryId}`,
  },
  order: {
    list: "order",
    cart: "order/cart",
    addOrder: "order",
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
