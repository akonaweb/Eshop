"use client";
import Stack from "@mui/material/Stack";

import {
  addCategory,
  Category,
  updateCategory,
  useCategoryDetailQuery,
} from "@/api/categories";
import useApiMutation from "@/api/core/useApiMutation";
import Data from "@/components/Data";
import Text from "@/components/form/fields/Text";
import { Form } from "@/components/form/Form";
import Submit from "@/components/form/Submit";
import { useCallback, useMemo } from "react";

type Props = {
  categoryId: string;
};
const Content = ({ categoryId }: Props) => {
  const isAdd = useMemo(() => isNaN(Number(categoryId)), [categoryId]);
  const response = useCategoryDetailQuery(categoryId);
  const categoryMutation = useApiMutation(
    isAdd ? addCategory : updateCategory,
    {
      onSuccess: () => {
        window.location.href = "/admin/categories";
      },
      onError: (error) => {
        // TODO: consume error - either 400 validation or 500 - probably modal
        console.info(error);
      },
    }
  );

  const handleSubmit = useCallback(
    (category: Category) => {
      categoryMutation.mutate(category);
    },
    [categoryMutation]
  );

  return (
    <Data response={response}>
      {(data) => {
        return (
          <>
            <h1>{isAdd ? "Add Category" : "Update Category"}</h1>

            <Form<Category> defaultValues={data!} onSubmit={handleSubmit}>
              {() => (
                <Stack spacing={2} sx={{ maxWidth: 400 }}>
                  <Text<Category> name="name" label="Category Name" />
                  <Submit text={isNaN(Number(categoryId)) ? "Add" : "Update"} />
                </Stack>
              )}
            </Form>
          </>
        );
      }}
    </Data>
  );
};

export default Content;
