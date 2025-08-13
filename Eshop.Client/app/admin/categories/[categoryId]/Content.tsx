"use client";
import Stack from "@mui/material/Stack";

import Text from "@/components/form/fields/Text";
import { Form } from "@/components/form/Form";
import { EditUserType } from "./page";
import Submit from "@/components/form/Submit";
import Data from "@/components/Data";
import { useCategoryDetailQuery } from "@/api/categories";

type Props = {
  categoryId: string;
};
const Content = ({ categoryId }: Props) => {
  const response = useCategoryDetailQuery(categoryId);

  return (
    <Data response={response}>
      {(data) => {
        return (
          <Form<EditUserType>
            defaultValues={data!}
            onSubmit={(data) => {
              console.info(data);
            }}
          >
            {() => (
              <Stack spacing={2} sx={{ maxWidth: 400 }}>
                <Text<EditUserType> name="name" label="Category Name" />
                <Submit text={isNaN(Number(categoryId)) ? "Add" : "Update"} />
              </Stack>
            )}
          </Form>
        );
      }}
    </Data>
  );
};

export default Content;
