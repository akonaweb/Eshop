"use client";

import {
  createContext,
  ReactNode,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";

import { CartItem } from "@/api/orders";

const localStorageKey = "eshop-cart";

type CartContextType = {
  items: CartItem[];
  onProductAdd: (item: CartItem) => void;
  onProductUpdate: (item: CartItem) => void;
  onProductRemove: (productId: number) => void;
  onCartEmpty: () => void;
};
const CartContext = createContext<CartContextType>(null!);
export const useCartContext = () => useContext(CartContext);

const getCartItems = (): CartItem[] =>
  JSON.parse(localStorage.getItem(localStorageKey)! ?? "[]");

type Props = {
  children: ReactNode;
};
const CartProvider = ({ children }: Props) => {
  const [items, setItems] = useState<CartItem[]>([]);
  const [isInitialized, setInitialized] = useState(false);

  useEffect(() => {
    setItems(getCartItems());
    setInitialized(true);
  }, []);

  useEffect(() => {
    if (!isInitialized) return;
    localStorage.setItem(localStorageKey, JSON.stringify(items));
  }, [isInitialized, items]);

  const onProductAdd = useCallback((item: CartItem) => {
    setItems((prev) => {
      const isExistingItem = prev.some((x) => x.productId === item.productId);
      const newItems: CartItem[] = isExistingItem
        ? prev.map((x) =>
            x.productId === item.productId
              ? { ...x, quantity: x.quantity + item.quantity }
              : x
          )
        : [...prev, item];

      return newItems;
    });
  }, []);

  const onProductRemove = useCallback((productId: number) => {
    setItems((prev) => {
      const newItems = prev.filter((x) => x.productId !== productId);
      return newItems;
    });
  }, []);

  const onProductUpdate = useCallback((item: CartItem) => {
    if (item.quantity <= 0) return;

    setItems((prev) => {
      const newItems = prev.map((x) =>
        x.productId === item.productId ? { ...x, quantity: item.quantity } : x
      );

      return newItems;
    });
  }, []);

  const onCartEmpty = useCallback(() => setItems([]), []);

  const value: CartContextType = useMemo(() => {
    return {
      items,
      onProductAdd,
      onProductUpdate,
      onProductRemove,
      onCartEmpty,
    };
  }, [items, onProductAdd, onProductUpdate, onProductRemove, onCartEmpty]);

  return <CartContext.Provider value={value}>{children}</CartContext.Provider>;
};

export default CartProvider;
